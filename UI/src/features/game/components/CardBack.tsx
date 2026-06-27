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

export const CardBack = ({ displayWidth, className }: CardBackProps) => {
  const scale = displayWidth / BACK_W;
  const displayHeight = Math.round(BACK_H * scale);

  return (
    <div
      className={cn("relative shrink-0", className)}
      style={{
        width: displayWidth,
        height: displayHeight,
        overflow: "hidden",
      }}
    >
      <div
        style={{
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
        }}
      />
    </div>
  );
};
