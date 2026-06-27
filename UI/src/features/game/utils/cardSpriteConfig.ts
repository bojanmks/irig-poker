import cardSprites from "@/assets/card_sprites.png";

import type { SuitName } from "../models/SuitName";

const CARD_W = 143;
const CARD_H = 206;

// Actual pixel positions of each card column in the sprite (non-uniform gaps)
const COL_X: number[] = [9, 156, 302, 448, 594, 741, 887, 1033, 1179, 1325, 1472, 1616, 1761];

// Actual pixel positions of each card row in the sprite (uniform 4px gaps)
const ROW_Y: number[] = [121, 331, 541, 751];

const SUIT_ROW: Record<SuitName, number> = {
  Hearts: 0,
  Diamonds: 1,
  Clubs: 2,
  Spades: 3,
};

const RANK_COL: Record<number, number> = {
  99: 0,  // Ace
  2: 1,   // Two
  3: 2,   // Three
  4: 3,   // Four
  5: 4,   // Five
  6: 5,   // Six
  7: 6,   // Seven
  8: 7,   // Eight
  9: 8,   // Nine
  10: 9,  // Ten
  12: 10, // Jack
  13: 11, // Queen
  14: 12, // King
};

export function getCardSpritePosition(suit: SuitName, rank: number) {
  const col = RANK_COL[rank] ?? 0;
  const row = SUIT_ROW[suit] ?? 0;

  return {
    x: COL_X[col],
    y: ROW_Y[row],
  };
}

export { CARD_H, CARD_W, cardSprites };
