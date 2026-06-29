namespace WebApi.Common.Features.Games.Models;

public record CallBluffResult(
    string GameCode,
    RoundResolvedNotification Resolution,
    string? EliminatedPlayerId,
    string? WinnerPlayerId,
    string? WinnerUsername,
    PublicGameState? UpdatedGameState,
    IReadOnlyDictionary<string, List<Card>> PlayerCards
);
