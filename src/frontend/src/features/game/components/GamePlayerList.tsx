import { useMemo, useState } from "react";
import { useTranslation } from "react-i18next";

import { Users } from "lucide-react";

import { Button } from "@/features/shared/components/shadcn/Button";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/features/shared/components/shadcn/Dialog";
import { useAppSelector } from "@/features/store/hooks";

import { GamePlayerCard } from "./GamePlayerCard";

export const GamePlayerList = () => {
    const { t } = useTranslation();
    const [dialogOpen, setDialogOpen] = useState(false);
    const gameState = useAppSelector((state) => state.gameState.gameState);
    const playerId = useAppSelector((state) => state.gameState.playerId);

    const players = useMemo(() => {
        if (!gameState) {
            return [];
        }

        const { playerOrder, currentTurnPlayerId } = gameState;
        const turnIndex = currentTurnPlayerId ? playerOrder.indexOf(currentTurnPlayerId) : -1;

        const order = turnIndex > -1
            ? [...playerOrder.slice(turnIndex), ...playerOrder.slice(0, turnIndex)]
            : playerOrder;
            
        return order.map(id => gameState.players[id]);
    }, [gameState]);

    const currentTurnPlayer = useMemo(() => {
        if (!gameState?.currentTurnPlayerId) return null;
        return gameState.players[gameState.currentTurnPlayerId];
    }, [gameState]);

    return (
        <>
            <div className="hidden lg:grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-3">
                {players.map((player) => (
                    <GamePlayerCard
                        key={player.playerId}
                        player={player}
                        isCurrentTurn={player.playerId === gameState?.currentTurnPlayerId}
                        isSelf={player.playerId === playerId}
                    />
                ))}
            </div>

            <div className="lg:hidden flex flex-col gap-3">
                {currentTurnPlayer && (
                    <GamePlayerCard
                        player={currentTurnPlayer}
                        isCurrentTurn
                        isSelf={currentTurnPlayer.playerId === playerId}
                    />
                )}
                <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
                    <DialogTrigger asChild>
                        <Button variant="outline" className="w-full gap-2">
                            <Users size={16} />
                            {t("game.viewAllPlayers")}
                        </Button>
                    </DialogTrigger>
                    <DialogContent className="sm:max-w-md">
                        <DialogHeader>
                            <DialogTitle>{t("game.allPlayers")}</DialogTitle>
                        </DialogHeader>
                        <div className="flex flex-col gap-2 max-h-[60dvh] overflow-y-auto p-1">
                            {players.map((player) => (
                                <GamePlayerCard
                                    key={player.playerId}
                                    player={player}
                                    isCurrentTurn={player.playerId === gameState?.currentTurnPlayerId}
                                    isSelf={player.playerId === playerId}
                                />
                            ))}
                        </div>
                    </DialogContent>
                </Dialog>
            </div>
        </>
    )
};
