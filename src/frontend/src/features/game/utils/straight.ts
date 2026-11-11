import { Rank } from "../models/Rank";

const RANK_SEQUENCE: Rank[] = [
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
  Rank.Ace,
];

export const STRAIGHT_TOP_RANKS: Rank[] = [
  Rank.Five,
  Rank.Six,
  Rank.Seven,
  Rank.Eight,
  Rank.Nine,
  Rank.Ten,
  Rank.Jack,
  Rank.Queen,
  Rank.King,
  Rank.Ace,
];

export function getStraightRanks(topRank: Rank): Rank[] {
  if (topRank === Rank.Five) {
    return [Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five];
  }

  const topIdx = RANK_SEQUENCE.indexOf(topRank);
  if (topIdx < 4) {
    return [];
  }

  return RANK_SEQUENCE.slice(topIdx - 4, topIdx + 1);
}
