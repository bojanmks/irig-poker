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
            var nextIndex = (currentIndex + 1) % PlayerOrder.Count;
            StartTurnPlayerId = PlayerOrder[nextIndex];
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
        var nextIndex = (currentIndex + 1) % PlayerOrder.Count;
        CurrentTurnPlayerId = PlayerOrder[nextIndex];
    }

    public Deck? Deck { get; private set; }
    public Dictionary<string, List<Card>> PlayerCards { get; } = [];

    public void CreateDeck()
    {
        Deck = new Deck();
        Deck.Shuffle();
    }

    public void DealCardsToAllPlayers()
    {
        if (Deck is null)
        {
            throw new InvalidOperationException("Deck has not been created");
        }

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

    public void SetClaim(string claimingPlayerId, HandType claimedHand, List<Rank> ranks)
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
    }

    public void ClearClaim()
    {
        CurrentClaimedHand = null;
        ClaimingPlayerId = null;
        Ranks = null;
    }

    public List<Card> GetAllCombinedCards()
    {
        return PlayerCards.SelectMany(kvp => kvp.Value).ToList();
    }

    public void RemovePlayer(string playerId)
    {
        ActivePlayerIds.Remove(playerId);
        PlayerOrder = [.. PlayerOrder.Where(id => id != playerId)];
        PlayerCards.Remove(playerId);

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

    public const int MaxCardCount = 7;
}
