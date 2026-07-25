import { useMemo } from "react";

import type { Card } from "../models/Card";
import { HandType } from "../models/HandType";
import { Rank } from "../models/Rank";
import type { Suit as SuitType } from "../models/Suit";
import { Suit } from "../models/Suit";

import { CardSprite } from "./CardSprite";

type ClaimedHandCardsProps = {
  handType: HandType;
  ranks: Rank[];
  suit?: SuitType | null;
  displayWidth?: number;
};

const OVERLAP_VISIBLE = 0.18;

const RANK_SEQUENCE: Rank[] = [
  Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six,
  Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten,
  Rank.Jack, Rank.Queen, Rank.King, Rank.Ace,
];

function getStraightRanks(topRank: Rank): Rank[] {
  if (topRank === Rank.Five) {
    return [Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five];
  }

  const topIdx = RANK_SEQUENCE.indexOf(topRank);
  if (topIdx < 4) return [];

  return RANK_SEQUENCE.slice(topIdx - 4, topIdx + 1);
}

const ALL_SUITS: SuitType[] = [Suit.Hearts, Suit.Diamonds, Suit.Clubs, Suit.Spades];

function distinctSuits(count: number, startOffset: number = 0): SuitType[] {
  return Array.from({ length: count }, (_, i) => ALL_SUITS[(startOffset + i) % 4]);
}

function generateCards(handType: HandType, ranks: Rank[], suit: SuitType): Card[] {
  switch (handType) {
    case HandType.HighCard:
      return [{ suit: suit, rank: ranks[0] }];
    case HandType.OnePair:
      return distinctSuits(2).map((s) => ({ suit: s, rank: ranks[0] }));
    case HandType.TwoPair:
      return [
        ...distinctSuits(2, 0).map((s) => ({ suit: s, rank: ranks[0] })),
        ...distinctSuits(2, 2).map((s) => ({ suit: s, rank: ranks[1] })),
      ];
    case HandType.ThreeOfAKind:
      return distinctSuits(3).map((s) => ({ suit: s, rank: ranks[0] }));
    case HandType.Straight:
    case HandType.StraightFlush:
      return getStraightRanks(ranks[0]).map((r) => ({ suit, rank: r }));
    case HandType.FullHouse:
      return [
        ...distinctSuits(3, 0).map((s) => ({ suit: s, rank: ranks[0] })),
        ...distinctSuits(2, 3).map((s) => ({ suit: s, rank: ranks[1] })),
      ];
    case HandType.FourOfAKind:
      return distinctSuits(4).map((s) => ({ suit: s, rank: ranks[0] }));
    default:
      return [];
  }
}

export const ClaimedHandCards = ({ handType, ranks, suit, displayWidth = 60 }: ClaimedHandCardsProps) => {
  const defaultSuit = suit ?? Suit.Hearts;
  const cardWidth = displayWidth;
  const overlapOffset = Math.round(cardWidth * OVERLAP_VISIBLE);
  const cardHeight = Math.round(206 * cardWidth / 143);

  const cards = useMemo(() => generateCards(handType, ranks, defaultSuit), [handType, ranks, defaultSuit]);

  const needsGrouping = handType === HandType.TwoPair || handType === HandType.FullHouse;

  if (needsGrouping) {
    const groupSize = handType === HandType.TwoPair ? 2 : 3;
    const group1 = cards.slice(0, groupSize);
    const group2 = cards.slice(groupSize);

    const group1Width = cardWidth + (group1.length - 1) * overlapOffset;
    const group2Width = cardWidth + (group2.length - 1) * overlapOffset;

    return (
      <div
        className="flex items-end gap-2"
        style={{ height: cardHeight }}
      >
        <div className="relative shrink-0" style={{ width: group1Width, height: cardHeight }}>
          {group1.map((card, i) => (
            <div
              key={i}
              className="absolute top-0"
              style={{ left: i * overlapOffset, zIndex: i }}
            >
              <CardSprite suit={card.suit} rank={card.rank} displayWidth={cardWidth} />
            </div>
          ))}
        </div>
        <div className="relative shrink-0" style={{ width: group2Width, height: cardHeight }}>
          {group2.map((card, i) => (
            <div
              key={i + groupSize}
              className="absolute top-0"
              style={{ left: i * overlapOffset, zIndex: i }}
            >
              <CardSprite suit={card.suit} rank={card.rank} displayWidth={cardWidth} />
            </div>
          ))}
        </div>
      </div>
    );
  }

  const totalWidth = cards.length > 1 ? cardWidth + (cards.length - 1) * overlapOffset : cardWidth;

  return (
    <div className="relative" style={{ width: totalWidth, height: cardHeight }}>
      {cards.map((card, i) => (
        <div
          key={i}
          className="absolute top-0"
          style={{ left: i * overlapOffset, zIndex: i }}
        >
          <CardSprite suit={card.suit} rank={card.rank} displayWidth={cardWidth} />
        </div>
      ))}
    </div>
  );
};
