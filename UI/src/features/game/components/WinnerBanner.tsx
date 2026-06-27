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
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";

import { resetGameState } from "../store/gameStateSlice";

export const WinnerBanner = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const winner = useAppSelector((state) => state.gameState.winner);
  const playerId = useAppSelector((state) => state.gameState.playerId);

  if (!winner) return null;

  const isSelf = winner.winnerPlayerId === playerId;

  const handleCreateNewGame = () => {
    dispatch(resetGameState());
    navigate("/");
  };

  return (
    <Dialog open>
      <DialogContent className="sm:max-w-md">
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
