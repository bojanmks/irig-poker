import cardSprites from "@/assets/card_sprites.png";

import { Rank } from "../models/Rank";
import { Suit } from "../models/Suit";

const CARD_W = 143;
const CARD_H = 206;

// Actual pixel positions of each card column in the sprite (non-uniform gaps)
const COL_X: number[] = [9, 156, 302, 448, 594, 741, 887, 1033, 1179, 1325, 1472, 1616, 1761];

// Actual pixel positions of each card row in the sprite (uniform 4px gaps)
const ROW_Y: number[] = [121, 331, 541, 751];

const SUIT_ROW: Record<Suit, number> = {
  [Suit.Hearts]: 0,
  [Suit.Diamonds]: 1,
  [Suit.Clubs]: 2,
  [Suit.Spades]: 3,
};

const RANK_COL: Record<Rank, number> = {
  [Rank.Ace]: 0,
  [Rank.Two]: 1,
  [Rank.Three]: 2,
  [Rank.Four]: 3,
  [Rank.Five]: 4,
  [Rank.Six]: 5,
  [Rank.Seven]: 6,
  [Rank.Eight]: 7,
  [Rank.Nine]: 8,
  [Rank.Ten]: 9,
  [Rank.Jack]: 10,
  [Rank.Queen]: 11,
  [Rank.King]: 12,
};

export function getCardSpritePosition(suit: Suit, rank: Rank) {
  const col = RANK_COL[rank];
  const row = SUIT_ROW[suit];

  return {
    x: COL_X[col],
    y: ROW_Y[row],
  };
}

export { CARD_H, CARD_W, cardSprites };
