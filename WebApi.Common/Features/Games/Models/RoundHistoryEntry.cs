namespace WebApi.Common.Features.Games.Models;

public record RoundHistoryEntry(
    string ClaimingPlayerId,
    HandType ClaimedHand,
    List<Rank> Ranks,
    Suit? Suit = null
);
