import { type CSSProperties,useMemo } from "react";

import cardSprites from "@/assets/card_sprites.png";
import { cn } from "@/lib/utils";

import type { SuitName } from "../models/SuitName";
import { CARD_H,CARD_W, getCardSpritePosition } from "../utils/cardSpriteConfig";

type CardSpriteProps = {
  suit: SuitName;
  rank: number;
  displayWidth?: number;
  className?: string;
};

const SPRITE_W = 1920;
const SPRITE_H = 1080;

export const CardSprite = ({ suit, rank, displayWidth = 75, className }: CardSpriteProps) => {
  const pos = useMemo(() => {
    return getCardSpritePosition(suit, rank);
  }, [suit, rank]);

  const scale = useMemo(() => {
    return displayWidth / CARD_W;
  }, [displayWidth]);

  const displayHeight = useMemo(() => {
    return Math.round(CARD_H * scale);
  }, [scale]);

  const wrapperStyle = useMemo<CSSProperties>(() => ({
    width: displayWidth,
    height: displayHeight,
    overflow: "hidden",
  }), [displayWidth, displayHeight]);

  const imageStyle = useMemo<CSSProperties>(() => ({
    width: SPRITE_W,
    height: SPRITE_H,
    backgroundImage: `url(${cardSprites})`,
    backgroundPosition: `-${pos.x}px -${pos.y}px`,
    transform: `scale(${scale})`,
    transformOrigin: "0 0",
  }), [pos, scale]);

  return (
    <div
      className={cn("shrink-0", className)}
      style={wrapperStyle}
    >
      <div
        style={imageStyle}
      />
    </div>
  );
};
