namespace IrigPoker.Common.Features.Games.Models;

public static class HandEvaluator
{
    private static readonly Rank[] StraightSequence =
    [
        Rank.Two,
        Rank.Three,
        Rank.Four,
        Rank.Five,
        Rank.Six,
        Rank.Seven,
        Rank.Eight,
        Rank.Nine,
        Rank.Ten,
        Rank.Jack,
        Rank.Queen,
        Rank.King,
        Rank.Ace
    ];

    public static int CompareRanks(Rank a, Rank b)
    {
        return ((int)a).CompareTo((int)b);
    }

    // The ranks a straight with the given top card is made of, or an empty list when the rank cannot top a straight.
    public static IReadOnlyList<Rank> GetStraightRanks(Rank topRank)
    {
        if (topRank == Rank.Five)
        {
            return [Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five];
        }

        var topIndex = Array.IndexOf(StraightSequence, topRank);
        if (topIndex < 4)
        {
            return [];
        }

        return StraightSequence[(topIndex - 4)..(topIndex + 1)];
    }

    public static bool HandExistsWithRanks(IReadOnlyList<Card> allCards, HandType handType, List<Rank> ranks, Suit? claimedSuit = null)
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
            HandType.StraightFlush => ranks.Count > 0 && HasStraightFlushWithTop(allCards, ranks[0], claimedSuit),
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
        var straightRanks = GetStraightRanks(topRank);
        if (straightRanks.Count != 5)
        {
            return false;
        }

        var presentRanks = cards.Select(c => c.Rank).ToHashSet();

        return straightRanks.All(presentRanks.Contains);
    }

    private static bool HasFullHouse(IReadOnlyList<Card> cards, Rank triple, Rank pair)
    {
        if (triple == pair) return false;
        return cards.Count(c => c.Rank == triple) >= 3 && cards.Count(c => c.Rank == pair) >= 2;
    }

    private static bool HasStraightFlushWithTop(IReadOnlyList<Card> cards, Rank topRank, Suit? claimedSuit)
    {
        var suits = cards.GroupBy(c => c.Suit);
        foreach (var suitGroup in suits)
        {
            if (claimedSuit.HasValue && suitGroup.Key != claimedSuit.Value)
            {
                continue;
            }

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
