namespace IrigPoker.Common.Features.Games.Models;

public record ClaimResult(
    PublicGameState GameState,
    ClaimNotification ClaimNotification
);
