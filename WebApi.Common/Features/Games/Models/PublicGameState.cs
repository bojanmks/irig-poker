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
);
