import { useMemo } from "react";
import { useTranslation } from "react-i18next";

import { X } from "lucide-react";

import {
  Avatar,
  AvatarFallback,
} from "@/features/shared/components/shadcn/Avatar";
import { Card } from "@/features/shared/components/shadcn/Card";
import { cn } from "@/lib/utils";

import { CardBack } from "./CardBack";

type GamePlayerCardProps = {
  player: {
    playerId: string;
    username: string;
    cardCount: number;
    isEliminated: boolean;
  };
  isCurrentTurn: boolean;
  isSelf: boolean;
};

export const GamePlayerCard = ({ player, isCurrentTurn, isSelf }: GamePlayerCardProps) => {
  const isEliminated = player.isEliminated;
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
    if (isSelf) {
      return "ring-accent";
    }

    if (isCurrentTurn) {
      return "ring-primary";
    }

    return "ring-border";
  }, [isSelf, isCurrentTurn]);

  return (
      <Card
        className={cn("flex flex-row items-center gap-3 p-3 transition-all", cardRingClass, isEliminated && "opacity-50")}
      >
      <Avatar className={cn("w-10 h-10 ring-2 shrink-0", avatarRingClass)}>
        <AvatarFallback className="text-sm">
          {player.username[0]?.toUpperCase()}
        </AvatarFallback>
      </Avatar>

      <div className="flex flex-col min-w-0">
        <span className="font-semibold text-sm truncate">{player.username}</span>
        {isSelf && (
          <span className="text-xs leading-tight text-accent">{t("game.you")}</span>
        )}
        {isCurrentTurn && (
          <span className="text-xs font-medium leading-tight text-primary">{t("game.currentTurn")}</span>
        )}
      </div>

      {!isEliminated && player.cardCount > 0 && (
        <div className="flex items-center gap-1 ml-auto shrink-0">
          <span className="text-sm font-medium tabular-nums">{player.cardCount}</span>
          <X size={12} />
          <CardBack displayWidth={20} className="opacity-80" />
        </div>
      )}
    </Card>
  );
};
