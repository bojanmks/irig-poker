using System.Collections.Concurrent;
using WebApi.Common.Features.Players.Models;

using static WebApi.Common.Features.Games.Helpers.HandStrengthHelper;

namespace WebApi.Common.Features.Games.Models;

public class GameState
{
    public required string GameCode { get; init; }

    public bool HasStarted { get; private set; } = false;
    public void MarkStarted()
    {
        if (HasStarted)
        {
            throw new InvalidOperationException("Game has already started");
        }

        if (Players.Count < 2)
        {
            throw new InvalidOperationException("Cannot start game with less than 2 players");
        }

        HasStarted = true;
    }

    public ConcurrentDictionary<string, Player> Players { get; } = new();

    public void InitializeCardCounts()
    {
        foreach (var playerId in Players.Keys)
        {
            Players[playerId].CardCount = 1;
        }
    }

    public HashSet<string> ActivePlayerIds { get; } = [];
    public List<string> PlayerOrder { get; private set; } = [];
    public void ShufflePlayerOrder()
    {
        var playerIds = PlayerOrder.ToArray();
        Random.Shared.Shuffle(playerIds);
        PlayerOrder = [.. playerIds];
    }

    public string? StartTurnPlayerId { get; private set; }

    public void StartNewRound()
    {
        if (!HasStarted)
        {
            throw new InvalidOperationException("Game has not started");
        }

        if (StartTurnPlayerId is null)
        {
            StartTurnPlayerId = PlayerOrder[0];
        }
        else
        {
            var currentIndex = PlayerOrder.IndexOf(StartTurnPlayerId);

            while (true)
            {
                var nextIndex = (currentIndex + 1) % PlayerOrder.Count;
                var nextId = PlayerOrder[nextIndex];

                if (Players[nextId].IsEliminated)
                {
                    currentIndex = nextIndex;
                    continue;
                }

                StartTurnPlayerId = PlayerOrder[nextIndex];
                break;
            }
        }

        ClearClaim();

        CurrentTurnPlayerId = StartTurnPlayerId;
    }

    public string? CurrentTurnPlayerId { get; private set; }
    public void NextTurn()
    {
        if (!HasStarted)
        {
            throw new InvalidOperationException("Game has not started");
        }

        if (CurrentTurnPlayerId is null)
        {
            throw new InvalidOperationException("Current turn player is not set");
        }

        var currentIndex = PlayerOrder.IndexOf(CurrentTurnPlayerId);

        while (true)
        {
            var nextIndex = (currentIndex + 1) % PlayerOrder.Count;
            var nextId = PlayerOrder[nextIndex];

            if (Players[nextId].IsEliminated)
            {
                currentIndex = nextIndex;
                continue;
            }

            CurrentTurnPlayerId = PlayerOrder[nextIndex];
            break;
        }
    }

    public Deck? Deck { get; private set; }
    public Dictionary<string, List<Card>> PlayerCards { get; } = [];

    public void CreateDeck()
    {
        Deck = new Deck();
    }

    public void DealCardsToAllPlayers()
    {
        if (Deck is null)
        {
            throw new InvalidOperationException("Deck has not been created");
        }

        Deck.Shuffle();

        foreach (var playerId in ActivePlayerIds)
        {
            var cards = Deck.Deal(Players[playerId].CardCount);
            PlayerCards[playerId] = cards;
        }
    }

    public void AddCardToPlayer(string playerId)
    {
        if (Deck is null)
        {
            throw new InvalidOperationException("Deck has not been created");
        }

        Players[playerId].CardCount++;
    }

    public HandType? CurrentClaimedHand { get; set; }
    public string? ClaimingPlayerId { get; set; }
    public List<Rank>? Ranks { get; private set; }
    public Suit? ClaimedSuit { get; private set; }

    public void SetClaim(string claimingPlayerId, HandType claimedHand, List<Rank> ranks, Suit? suit = null)
    {
        if (CurrentClaimedHand.HasValue && ClaimingPlayerId is not null && Ranks is not null)
        {
            if (!IsStrongerThan(claimedHand, ranks, CurrentClaimedHand.Value, Ranks))
            {
                throw new InvalidOperationException("Must claim a stronger hand than the current claim");
            }
        }

        ClaimingPlayerId = claimingPlayerId;
        CurrentClaimedHand = claimedHand;
        Ranks = ranks;
        ClaimedSuit = suit;
    }

    public void ClearClaim()
    {
        CurrentClaimedHand = null;
        ClaimingPlayerId = null;
        Ranks = null;
        ClaimedSuit = null;
    }

    public List<Card> GetAllCombinedCards()
    {
        return PlayerCards.SelectMany(kvp => kvp.Value).ToList();
    }

    public void EliminatePlayer(string playerId)
    {
        ActivePlayerIds.Remove(playerId);
        PlayerCards.Remove(playerId);
        Players[playerId].IsEliminated = true;

        if (CurrentTurnPlayerId == playerId)
        {
            NextTurn();
        }
    }

    public void AddCardsToPlayer(string playerId, List<Card> cards)
    {
        if (!PlayerCards.TryGetValue(playerId, out var existing))
        {
            existing = [];
            PlayerCards[playerId] = existing;
        }

        existing.AddRange(cards);
        Players[playerId].CardCount += cards.Count;
    }

    public static readonly int MaxPlayers = 15;
    public int MaxCardCount { get; private set; } = 7;
    
    public void UpdateCardCountThreshold()
    {
        MaxCardCount = CalculateMaxCardCount(ActivePlayerIds.Count);
    }

    private static int CalculateMaxCardCount(int playerCount)
    {
        for (var m = 7; m >= 4; m--)
        {
            if (playerCount * m + m * (m - 1) / 2 <= 45)
                return m;
        }

        return 4;
    }
}
