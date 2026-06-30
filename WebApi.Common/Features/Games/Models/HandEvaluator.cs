namespace WebApi.Common.Features.Games.Models;

public static class HandEvaluator
{
    public static int CompareRanks(Rank a, Rank b)
    {
        return ((int)a).CompareTo((int)b);
    }

    public static bool HandExistsWithRanks(IReadOnlyList<Card> allCards, HandType handType, List<Rank> ranks)
    {
        return handType switch
        {
            HandType.HighCard => ranks.Count > 0 && allCards.Any(c => c.Rank == ranks[0]),
            HandType.OnePair => ranks.Count > 0 && HasNOfAKind(allCards, 2, ranks[0]),
            HandType.TwoPair => ranks.Count > 1 && HasTwoPair(allCards, ranks[0], ranks[1]),
            HandType.Straight => ranks.Count > 0 && HasStraightWithTop(allCards, ranks[0]),
            HandType.ThreeOfAKind => ranks.Count > 0 && HasNOfAKind(allCards, 3, ranks[0]),
            HandType.FullHouse => ranks.Count > 1 && HasFullHouse(allCards, ranks[0], ranks[1]),
            HandType.FourOfAKind => ranks.Count > 0 && HasNOfAKind(allCards, 4, ranks[0]),
            HandType.StraightFlush => ranks.Count > 0 && HasStraightFlushWithTop(allCards, ranks[0]),
            HandType.RoyalFlush => HasRoyalFlush(allCards),
            _ => false
        };
    }

    private static bool HasNOfAKind(IReadOnlyList<Card> cards, int n, Rank rank)
    {
        return cards.Count(c => c.Rank == rank) >= n;
    }

    private static bool HasTwoPair(IReadOnlyList<Card> cards, Rank first, Rank second)
    {
        if (first == second) return false;
        return cards.Count(c => c.Rank == first) >= 2 && cards.Count(c => c.Rank == second) >= 2;
    }

    private static bool HasStraightWithTop(IReadOnlyList<Card> cards, Rank topRank)
    {
        var topValue = (int)topRank;
        var values = cards.Select(c => (int)c.Rank).Distinct().OrderBy(v => v).ToArray();
        if (values.Length < 5) return false;

        if (values.Contains(topValue)
            && values.Contains(topValue - 1)
            && values.Contains(topValue - 2)
            && values.Contains(topValue - 3)
            && values.Contains(topValue - 4))
            return true;

        if (topRank == Rank.Five
            && values.Contains(14)
            && values.Contains(2)
            && values.Contains(3)
            && values.Contains(4)
            && values.Contains(5))
            return true;

        return false;
    }

    private static bool HasFullHouse(IReadOnlyList<Card> cards, Rank triple, Rank pair)
    {
        if (triple == pair) return false;
        return cards.Count(c => c.Rank == triple) >= 3 && cards.Count(c => c.Rank == pair) >= 2;
    }

    private static bool HasStraightFlushWithTop(IReadOnlyList<Card> cards, Rank topRank)
    {
        var suits = cards.GroupBy(c => c.Suit);
        foreach (var suitGroup in suits)
        {
            if (suitGroup.Count() >= 5 && HasStraightWithTop(suitGroup.ToList(), topRank))
                return true;
        }
        return false;
    }

    private static bool HasRoyalFlush(IReadOnlyList<Card> cards)
    {
        var suits = cards.GroupBy(c => c.Suit);
        foreach (var suitGroup in suits)
        {
            var values = suitGroup.Select(c => c.Rank).ToHashSet();

            if (values.Contains(Rank.Ten) && values.Contains(Rank.Jack) && values.Contains(Rank.Queen) && values.Contains(Rank.King) && values.Contains(Rank.Ace))
            {
                return true;
            }
        }
        return false;
    }
}
