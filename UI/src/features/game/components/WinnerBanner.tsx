import { useCallback, useMemo } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

import { Button } from "@/features/shared/components/shadcn/Button";
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/features/shared/components/shadcn/Dialog";
import { useAppSelector } from "@/features/store/hooks";

import type { GameWonNotification } from "../models/GameWonNotification";

type WinnerBannerProps = {
  winner: GameWonNotification | null;
};

export const WinnerBanner = ({ winner }: WinnerBannerProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const playerId = useAppSelector((state) => state.gameState.playerId);

  const isSelf = useMemo(() => {
    return winner && winner.winnerPlayerId === playerId;
  }, [winner, playerId]);

  const handleCreateNewGame = useCallback(() => {
    navigate("/");
  }, [navigate]);

  if (!winner) {
    return null;
  }

  return (
    <Dialog open>
      <DialogContent className="sm:max-w-md" hideClose={true}>
        <DialogHeader>
          <DialogTitle className="text-center">
            {isSelf ? t("game.youWon") : t("game.playerWon", { username: winner.winnerUsername })}
          </DialogTitle>
        </DialogHeader>
        <DialogFooter className="sm:justify-center">
          <Button onClick={handleCreateNewGame}>
            {t("game.createNewGame")}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
