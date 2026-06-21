using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Models;

public record PublicGameStateDto(
    string GameCode,
    bool HasStarted,
    IReadOnlyDictionary<string, PlayerDto> Players,
    IReadOnlyList<string> PlayerOrder,
    string? CurrentTurnPlayerId
);