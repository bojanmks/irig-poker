import cardSprites from "@/assets/card_sprites.png";

const SPRITE_X = 9;
const SPRITE_Y = 121;
const CARD_W = 143;
const CARD_H = 206;
const GAP_X = 4;
const GAP_Y = 4;

type SuitName = "Hearts" | "Diamonds" | "Clubs" | "Spades";

const SUIT_ROW: Record<SuitName, number> = {
  Hearts: 0,
  Diamonds: 1,
  Clubs: 2,
  Spades: 3,
};

const RANK_COL: Record<number, number> = {
  99: 0, // Ace
  2: 1,
  3: 2,
  4: 3,
  5: 4,
  6: 5,
  7: 6,
  8: 7,
  9: 8,
  10: 9,
  12: 10, // Jack
  13: 11, // Queen
  14: 12, // King
};

export function getCardSpritePosition(suit: string, rank: number) {
  const row = SUIT_ROW[suit as SuitName] ?? 0;
  const col = RANK_COL[rank] ?? 0;

  const x = SPRITE_X + col * (CARD_W + GAP_X);
  const y = SPRITE_Y + row * (CARD_H + GAP_Y);

  return { x, y };
}

export function getCardCssBackground(suit: string, rank: number) {
  const pos = getCardSpritePosition(suit, rank);
  return {
    backgroundImage: `url(${cardSprites})`,
    backgroundPosition: `-${pos.x}px -${pos.y}px`,
    backgroundSize: `${1920}px ${1080}px`,
    width: `${CARD_W}px`,
    height: `${CARD_H}px`,
  };
}

export { CARD_H,CARD_W, cardSprites };
