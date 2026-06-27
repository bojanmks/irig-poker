import { useTranslation } from "react-i18next";

import { useAppSelector } from "@/features/store/hooks";

import { CardSprite } from "./CardSprite";

export const PlayerHand = () => {
  const { t } = useTranslation();
  const cards = useAppSelector((state) => state.gameState.cards);

  if (cards.length === 0) return null;

  return (
    <div className="flex flex-col items-center gap-3">
      <h2 className="text-sm font-semibold text-muted-foreground uppercase tracking-wide">
        {t("game.yourHand")}
      </h2>
      <div className="flex flex-col items-center gap-2">
        {cards.map((card, i) => (
          <CardSprite key={i} suit={card.suit} rank={card.rank} />
        ))}
      </div>
    </div>
  );
};
