import { type CSSProperties,useMemo } from "react";

import cardBackSprites from "@/assets/card_sprites_additional.png";
import { cn } from "@/lib/utils";

type CardBackProps = {
  displayWidth: number;
  className?: string;
};

const BACK_X = 169;
const BACK_Y = 181;
const BACK_W = 503;
const BACK_H = 727;
const SPRITE_W = 1920;
const SPRITE_H = 1080;

const HighResCardBack = ({ displayWidth, className, displayHeight }: CardBackProps & { displayHeight: number }) => {
  const scale = useMemo(() => {
    return displayWidth / BACK_W;
  }, [displayWidth]);

  const wrapperStyle = useMemo<CSSProperties>(() => ({
    width: displayWidth,
    height: displayHeight,
    overflow: "hidden",
  }), [displayWidth, displayHeight]);

  const imageStyle = useMemo<CSSProperties>(() => ({
    position: "absolute",
    top: 0,
    left: 0,
    width: SPRITE_W,
    height: SPRITE_H,
    backgroundImage: `url(${cardBackSprites})`,
    backgroundPosition: `-${BACK_X}px -${BACK_Y}px`,
    transform: `scale(${scale})`,
    transformOrigin: "0 0",
    willChange: "transform",
  }), [scale]);

  return (
    <div
      className={cn("relative shrink-0", className)}
      style={wrapperStyle}
    >
      <div
        style={imageStyle}
      />
    </div>
  );
}

const SmallResCardBack = ({ displayWidth, className, displayHeight }: CardBackProps & { displayHeight: number }) => {
  return (
      <div
        className={cn("shrink-0 rounded-xs border border-white bg-red-700", className)}
        style={{ width: displayWidth, height: displayHeight }}
      />
    );
}

export const CardBack = ({ displayWidth, className }: CardBackProps) => {
  const displayHeight = useMemo(() => {
    return Math.round(displayWidth * (BACK_H / BACK_W));
  }, [displayWidth]);

  if (displayWidth < 45) {
    return <SmallResCardBack displayWidth={displayWidth} displayHeight={displayHeight} className={className} />
  }

  return <HighResCardBack displayWidth={displayWidth} displayHeight={displayHeight} className={className} />
};
