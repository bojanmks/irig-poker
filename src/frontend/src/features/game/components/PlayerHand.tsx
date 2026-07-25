import { type CSSProperties,useMemo } from "react";
import { useTranslation } from "react-i18next";

import { useAppSelector } from "@/features/store/hooks";

import { CardSprite } from "./CardSprite";

const OVERLAP_VISIBLE = 0.18;
const CARD_WIDTH = 90;

export const PlayerHand = () => {
  const { t } = useTranslation();
  const cards = useAppSelector((state) => state.gameState.cards);
  const gameState = useAppSelector((state) => state.gameState.gameState);
  const playerId = useAppSelector((state) => state.gameState.playerId);

  const isEliminated = playerId && gameState?.players[playerId]?.isEliminated;

  const overlapOffset = useMemo(() => {
    if (cards.length <= 1) {
      return 0;
    }

    return Math.round(CARD_WIDTH * OVERLAP_VISIBLE);
  }, [cards.length]);

  const cardsWrapperStyle = useMemo<CSSProperties>(() => ({
    width: cards.length > 1 ? CARD_WIDTH + (cards.length - 1) * overlapOffset : CARD_WIDTH,
    height: Math.round(206 * CARD_WIDTH / 143)
  }), [cards.length, overlapOffset]);

  if (isEliminated) {
    return (
      <div className="flex items-center justify-center h-full">
        <p className="text-sm font-semibold text-red-600 dark:text-red-400 text-center">
          {t("game.youAreEliminated")}
        </p>
      </div>
    );
  }

  if (cards.length === 0) {
    return null;
  }

  return (
    <div className="flex flex-col items-center gap-3">
      <h2 className="text-sm font-semibold text-muted-foreground uppercase tracking-wide">
        {t("game.yourHand")}
      </h2>
      <div
        className="relative"
        style={cardsWrapperStyle}>
        {cards.map((card, i) => (
          <div
            key={i}
            className="absolute top-0"
            style={{
              left: i * overlapOffset,
              zIndex: i,
            }}
          >
            <CardSprite suit={card.suit} rank={card.rank} displayWidth={CARD_WIDTH} />
          </div>
        ))}
      </div>
    </div>
  );
};
