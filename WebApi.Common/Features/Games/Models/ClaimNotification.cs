namespace WebApi.Common.Features.Games.Models;

public record ClaimNotification(
    string ClaimingPlayerId,
    HandType ClaimedHand,
    List<Rank> Ranks,
    Suit? Suit = null
);
