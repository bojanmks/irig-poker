using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Models;

public record PublicGameState(
    string GameCode,
    bool HasStarted,
    IReadOnlyDictionary<string, Player> Players,
    IReadOnlyList<string> PlayerOrder,
    string? CurrentTurnPlayerId,
    HandType? CurrentClaimedHand,
    string? ClaimingPlayerId,
    List<Rank>? Ranks,
    Suit? ClaimedSuit,
    int MaxCardCount,
    IReadOnlyList<RoundHistoryEntry> RoundHistory
)
{
    public static PublicGameState FromGameState(GameState gameState) => new(
        gameState.GameCode,
        gameState.HasStarted,
        gameState.Players,
        gameState.PlayerOrder,
        gameState.CurrentTurnPlayerId,
        gameState.CurrentClaimedHand,
        gameState.ClaimingPlayerId,
        gameState.Ranks,
        gameState.ClaimedSuit,
        gameState.MaxCardCount,
        gameState.RoundHistory
    );
}
