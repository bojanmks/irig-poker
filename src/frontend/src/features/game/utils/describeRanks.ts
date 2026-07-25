import { HandType } from "../models/HandType";
import { Rank } from "../models/Rank";
import { Suit } from "../models/Suit";

const suitSymbols: Record<number, string> = {
  [Suit.Hearts]: "♥",
  [Suit.Diamonds]: "♦",
  [Suit.Clubs]: "♣",
  [Suit.Spades]: "♠",
};

export function rankLabel(rank: Rank): string {
  switch (rank) {
    case Rank.Two: return "2";
    case Rank.Three: return "3";
    case Rank.Four: return "4";
    case Rank.Five: return "5";
    case Rank.Six: return "6";
    case Rank.Seven: return "7";
    case Rank.Eight: return "8";
    case Rank.Nine: return "9";
    case Rank.Ten: return "10";
    case Rank.Jack: return "J";
    case Rank.Queen: return "Q";
    case Rank.King: return "K";
    case Rank.Ace: return "A";
    default:
      rank satisfies never;
      return "";
  }
}

export function describeRanks(handType: HandType, ranks: Rank[], suit?: number | null): string {
  if (ranks.length === 0) return "";
  if (handType === HandType.TwoPair) {
    return `${rankLabel(ranks[0])} & ${rankLabel(ranks[1])}`;
  }
  if (handType === HandType.FullHouse) {
    return `3x${rankLabel(ranks[0])} & 2x${rankLabel(ranks[1])}`;
  }
  const label = rankLabel(ranks[0]);
  if (handType === HandType.StraightFlush && suit !== null && suit !== undefined) {
    return `${label}${suitSymbols[suit] ?? ""}`;
  }
  return label;
}
