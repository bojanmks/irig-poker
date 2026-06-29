namespace WebApi.Common.Features.Games.Models;

public static class HandEvaluator
{
    public static int CompareRanks(Rank a, Rank b)
    {
        return StraightValue(a).CompareTo(StraightValue(b));
    }

    public static int StraightValue(Rank rank) => rank switch
    {
        Rank.Jack => 11,
        Rank.Queen => 12,
        Rank.King => 13,
        Rank.Ace => 14,
        _ => (int)rank
    };

    public static bool HandExists(IReadOnlyList<Card> allCards, HandType handType)
    {
        return handType switch
        {
            HandType.HighCard => true,
            HandType.OnePair => HasNOfAKind(allCards, 2),
            HandType.TwoPair => HasTwoPair(allCards),
            HandType.Straight => HasStraight(allCards),
            HandType.ThreeOfAKind => HasNOfAKind(allCards, 3),
            HandType.FullHouse => HasFullHouse(allCards),
            HandType.FourOfAKind => HasNOfAKind(allCards, 4),
            HandType.StraightFlush => HasStraightFlush(allCards),
            HandType.RoyalFlush => HasRoyalFlush(allCards),
            _ => false
        };
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

    private static bool HasNOfAKind(IReadOnlyList<Card> cards, int n)
    {
        return cards.GroupBy(c => c.Rank).Any(g => g.Count() >= n);
    }

    private static bool HasNOfAKind(IReadOnlyList<Card> cards, int n, Rank rank)
    {
        return cards.Count(c => c.Rank == rank) >= n;
    }

    private static bool HasTwoPair(IReadOnlyList<Card> cards)
    {
        return cards.GroupBy(c => c.Rank).Count(g => g.Count() >= 2) >= 2;
    }

    private static bool HasTwoPair(IReadOnlyList<Card> cards, Rank first, Rank second)
    {
        if (first == second) return false;
        return cards.Count(c => c.Rank == first) >= 2 && cards.Count(c => c.Rank == second) >= 2;
    }

    private static int[] GetStraightValues(IReadOnlyList<Card> cards)
    {
        return cards.Select(c => StraightValue(c.Rank)).Distinct().OrderBy(v => v).ToArray();
    }

    private static bool HasStraight(IReadOnlyList<Card> cards)
    {
        var values = GetStraightValues(cards);
        if (values.Length < 5) return false;

        for (int i = 0; i <= values.Length - 5; i++)
        {
            if (values[i + 4] - values[i] == 4) return true;
        }

        if (values.Contains(14) && values.Contains(2) && values.Contains(3) && values.Contains(4) && values.Contains(5))
            return true;

        return false;
    }

    private static bool HasStraightWithTop(IReadOnlyList<Card> cards, Rank topRank)
    {
        var topValue = StraightValue(topRank);
        var values = GetStraightValues(cards);
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

    private static bool HasFullHouse(IReadOnlyList<Card> cards)
    {
        var groups = cards.GroupBy(c => c.Rank).Select(g => g.Count()).ToList();
        return groups.Any(c => c >= 3) && groups.Any(c => c >= 2);
    }

    private static bool HasFullHouse(IReadOnlyList<Card> cards, Rank triple, Rank pair)
    {
        if (triple == pair) return false;
        return cards.Count(c => c.Rank == triple) >= 3 && cards.Count(c => c.Rank == pair) >= 2;
    }

    private static bool HasStraightFlush(IReadOnlyList<Card> cards)
    {
        var suits = cards.GroupBy(c => c.Suit);
        foreach (var suitGroup in suits)
        {
            if (suitGroup.Count() >= 5 && HasStraight(suitGroup.ToList()))
                return true;
        }
        return false;
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
            var values = suitGroup.Select(c => StraightValue(c.Rank)).ToHashSet();
            if (values.Contains(10) && values.Contains(11) && values.Contains(12) && values.Contains(13) && values.Contains(14))
                return true;
        }
        return false;
    }
}
