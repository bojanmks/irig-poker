import cardSprites from "@/assets/card_sprites.png";
import { cn } from "@/lib/utils";

import { CARD_H,CARD_W, getCardSpritePosition } from "../utils/cardSpriteConfig";

type CardSpriteProps = {
  suit: string;
  rank: number;
  displayWidth?: number;
  className?: string;
};

const SPRITE_W = 1920;
const SPRITE_H = 1080;

export const CardSprite = ({ suit, rank, displayWidth = 75, className }: CardSpriteProps) => {
  const pos = getCardSpritePosition(suit, rank);
  const scale = displayWidth / CARD_W;
  const displayHeight = Math.round(CARD_H * scale);

  return (
    <div
      className={cn("shrink-0", className)}
      style={{
        width: displayWidth,
        height: displayHeight,
        overflow: "hidden",
      }}
    >
      <div
        style={{
          width: SPRITE_W,
          height: SPRITE_H,
          backgroundImage: `url(${cardSprites})`,
          backgroundPosition: `-${pos.x}px -${pos.y}px`,
          transform: `scale(${scale})`,
          transformOrigin: "0 0",
        }}
      />
    </div>
  );
};
