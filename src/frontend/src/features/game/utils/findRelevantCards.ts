import type { Card } from "../models/Card";
import { HandType } from "../models/HandType";
import type { Rank } from "../models/Rank";
import type { Suit } from "../models/Suit";

import { getStraightRanks } from "./straight";

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
      const needed = getStraightRanks(ranks[0]);
      const neededSet = new Set(needed);
      return cards.filter((c) => neededSet.has(c.rank));
    }

    case HandType.StraightFlush: {
      const suited = suit !== null ? cards.filter((c) => c.suit === suit) : cards;
      const needed = getStraightRanks(ranks[0]);
      const neededSet = new Set(needed);
      return suited.filter((c) => neededSet.has(c.rank));
    }

    default:
      return [];
  }
}
