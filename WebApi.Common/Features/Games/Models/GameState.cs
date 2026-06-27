using System.Collections.Concurrent;
using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Models;

public class GameState
{
    public required string GameCode { get; init; }

    public bool HasStarted { get; private set; } = false;
    public void Start()
    {
        HasStarted = true;
    }

    public ConcurrentDictionary<string, Player> Players { get; } = new();
    public HashSet<string> ActivePlayerIds { get; } = [];
    public List<string> PlayerOrder { get; private set; } = [];
    public void ShufflePlayerOrder()
    {
        var playerIds = PlayerOrder.ToArray();
        Random.Shared.Shuffle(playerIds);
        PlayerOrder = [.. playerIds];
    }

    public string? CurrentTurnPlayerId { get; set; }
    public void NextTurn()
    {
        if (PlayerOrder.Count == 0)
        {
            throw new InvalidOperationException("No players in the game");
        }

        if (CurrentTurnPlayerId is null)
        {
            CurrentTurnPlayerId = PlayerOrder[0];
            return;
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

    public void DealCardsToAllPlayers(int count)
    {
        if (Deck is null)
        {
            throw new InvalidOperationException("Deck has not been created");
        }

        foreach (var playerId in ActivePlayerIds)
        {
            var cards = Deck.Deal(count);
            PlayerCards[playerId] = cards;
            Players[playerId].CardCount += count;
        }
    }

    public void DealCardToPlayer(string playerId)
    {
        if (Deck is null)
        {
            throw new InvalidOperationException("Deck has not been created");
        }

        if (!PlayerCards.TryGetValue(playerId, out var cards))
        {
            cards = [];
            PlayerCards[playerId] = cards;
        }

        cards.Add(Deck.DealOne());
        Players[playerId].CardCount++;
    }
}