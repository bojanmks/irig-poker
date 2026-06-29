using WebApi.Common.Features.Games.Models;

namespace WebApi.Common.Features.Games.Helpers;

public static class HandStrengthHelper
{
    public static bool IsStrongerThan(HandType handType, List<Rank> ranks, HandType otherHandType, List<Rank> otherRanks)
    {
        if (handType != otherHandType)
        {
            return handType > otherHandType;
        }

        for (int i = 0; i < ranks.Count && i < otherRanks.Count; i++)
        {
            var cmp = HandEvaluator.CompareRanks(ranks[i], otherRanks[i]);
            if (cmp != 0) return cmp > 0;
        }

        return ranks.Count > otherRanks.Count;
    }
}
