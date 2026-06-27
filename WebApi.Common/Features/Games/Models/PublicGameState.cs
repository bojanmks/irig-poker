using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Models;

public record PublicGameState(
    string GameCode,
    bool HasStarted,
    IReadOnlyDictionary<string, Player> Players,
    IReadOnlyList<string> PlayerOrder,
    string? CurrentTurnPlayerId
);