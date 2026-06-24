using System.Collections.Concurrent;
using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Models;

public class GameState
{
    public required string GameCode { get; set; }
    public bool HasStarted { get; set; } = false;
    public ConcurrentDictionary<string, Player> Players { get; } = new();
    public List<string> PlayerOrder { get; set; } = [];
    public string? CurrentTurnPlayerId { get; set; }
}