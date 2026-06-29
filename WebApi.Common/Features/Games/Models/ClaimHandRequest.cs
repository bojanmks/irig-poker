namespace WebApi.Common.Features.Games.Models;

public record ClaimHandRequest(HandType ClaimedHand, List<Rank> Ranks);
