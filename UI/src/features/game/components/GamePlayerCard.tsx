import { useMemo } from "react";
import { useTranslation } from "react-i18next";

import { X } from "lucide-react";

import {
  Avatar,
  AvatarFallback,
} from "@/features/shared/components/shadcn/Avatar";
import {
  Card,
  CardContent,
} from "@/features/shared/components/shadcn/Card";

import { CardBack } from "./CardBack";

type GamePlayerCardProps = {
  player: {
    playerId: string;
    username: string;
    cardCount: number;
  };
  isCurrentTurn: boolean;
  isSelf: boolean;
};

export const GamePlayerCard = ({ player, isCurrentTurn, isSelf }: GamePlayerCardProps) => {
  const { t } = useTranslation();

  const cardRingClass = useMemo(() => {
    if (isCurrentTurn) {
      return "ring-2 ring-primary shadow-lg shadow-primary/20";
    }

    if (isSelf) {
      return "ring-1 ring-accent";
    }

    return "";
  }, [isSelf, isCurrentTurn]);

  const avatarRingClass = useMemo(() => {
    if (isCurrentTurn) {
      return "ring-primary";
    }

    if (isSelf) {
      return "ring-accent";
    }

    return "ring-border";
  }, [isSelf, isCurrentTurn]);

  return (
    <Card
      className={`flex flex-col items-center gap-4 p-5 transition-all ${cardRingClass}`}
    >
      <Avatar className={`w-14 h-14 ring-2 ${avatarRingClass}`}>
        <AvatarFallback className="text-lg">
          {player.username[0]?.toUpperCase()}
        </AvatarFallback>
      </Avatar>
      {player.cardCount > 0 && (
        <div className="flex items-center gap-1.5">
          <span className="text-sm font-medium tabular-nums">{player.cardCount}</span>
          <X size={15} />
          <CardBack displayWidth={28} className="opacity-80" />
        </div>
      )}
      <CardContent className="flex flex-col items-center gap-1 p-0">
        <span className="font-semibold text-base">{player.username}</span>
        {isSelf && (
          <span className="text-xs text-accent">{t("game.you")}</span>
        )}
        {isCurrentTurn && (
          <span className="text-xs font-medium text-primary">{t("game.currentTurn")}</span>
        )}
      </CardContent>
    </Card>
  );
};
