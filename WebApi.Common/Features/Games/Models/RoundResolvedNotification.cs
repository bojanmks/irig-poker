namespace WebApi.Common.Features.Games.Models;

public record RoundResolvedNotification(
    string ClaimingPlayerId,
    HandType ClaimedHand,
    List<Rank> Ranks,
    string CallingPlayerId,
    bool WasTruthful,
    string LosingPlayerId,
    int LosingPlayerNewCardCount,
    IReadOnlyDictionary<string, List<Card>> AllPlayerCards,
    string? EliminatedPlayerId,
    string? WinnerPlayerId,
    string? WinnerUsername
);
