import cardSprites from "@/assets/card_sprites.png";
import { cn } from "@/lib/utils";

import { getCardSpritePosition } from "../utils/cardSpriteConfig";

type CardSpriteProps = {
  suit: string;
  rank: number;
  displayWidth?: number;
  className?: string;
};

const SPRITE_W = 1920;
const SPRITE_H = 1080;
const CARD_W = 143;
const CARD_H = 206;

export const CardSprite = ({ suit, rank, displayWidth = 75, className }: CardSpriteProps) => {
  const pos = getCardSpritePosition(suit, rank);
  const scale = displayWidth / CARD_W;
  const displayHeight = Math.round(CARD_H * scale);

  return (
    <div
      className={cn("shrink-0 bg-no-repeat shadow-sm", className)}
      style={{
        width: displayWidth,
        height: displayHeight,
        backgroundImage: `url(${cardSprites})`,
        backgroundPosition: `-${pos.x * scale}px -${pos.y * scale}px`,
        backgroundSize: `${SPRITE_W * scale}px ${SPRITE_H * scale}px`,
      }}
    />
  );
};
