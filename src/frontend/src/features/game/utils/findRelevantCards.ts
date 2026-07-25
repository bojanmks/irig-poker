import type { Card } from "../models/Card";
import { HandType } from "../models/HandType";
import { Rank } from "../models/Rank";
import type { Suit } from "../models/Suit";

const allRanks: Rank[] = [
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

function getStraightNeededRanks(topRank: Rank): Rank[] {
  if (topRank === Rank.Five) {
    return [Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five];
  }

  const topRankIndex = allRanks.indexOf(topRank);
  const needed: Rank[] = [];
  for (let i = topRankIndex; i > topRankIndex - 5; i--) {
    needed.push(allRanks[i]);
  }
  return needed;
}

export function findRelevantCards(
  cards: Card[],
  handType: HandType,
  ranks: Rank[],
  suit: Suit | null,
): Card[] {
  switch (handType) {
    case HandType.HighCard:
    case HandType.OnePair:
    case HandType.ThreeOfAKind:
    case HandType.FourOfAKind:
      return cards.filter((c) => c.rank === ranks[0]);

    case HandType.TwoPair:
    case HandType.FullHouse:
      return cards.filter((c) => c.rank === ranks[0] || c.rank === ranks[1]);

    case HandType.Straight: {
      const needed = getStraightNeededRanks(ranks[0]);
      const neededSet = new Set(needed);
      return cards.filter((c) => neededSet.has(c.rank));
    }

    case HandType.StraightFlush: {
      const suited = suit !== null ? cards.filter((c) => c.suit === suit) : cards;
      const needed = getStraightNeededRanks(ranks[0]);
      const neededSet = new Set(needed);
      return suited.filter((c) => neededSet.has(c.rank));
    }

    default:
      return [];
  }
}
