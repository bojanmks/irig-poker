import { useMemo } from "react";
import { useTranslation } from "react-i18next";

import { Dot, UsersIcon } from "lucide-react";

import { useAppSelector } from "@/features/store/hooks";

import { GameLobbyPlayerCard } from "./GameLobbyPlayerCard";

export const GameLobbyPlayerList = () => {
    const gameState = useAppSelector((state) => state.gameState.gameState);
    const playerId = useAppSelector((state) => state.gameState.playerId);
    const { t } = useTranslation();

    const players = useMemo(() => {
        if (!gameState) {
            return [];
        }

        return gameState.playerOrder.map(po => gameState.players[po]);
    }, [gameState]);

    if (!gameState) return null;

    return (
        <>
            <div className="flex items-center justify-between mb-4">
                <div className="flex items-center gap-2">
                    <UsersIcon className="w-5 h-5 text-muted-foreground" />
                    <span className="font-semibold">{t("game.players")}</span>
                </div>
                <div className="flex items-center text-sm text-muted-foreground">
                    {t("game.playerCount", {
                        count: gameState.playerOrder.length
                    })}

                    <Dot size={15} />
                    
                    {t("game.eliminatedAt", {
                        max: gameState.maxCardCount
                    })}
                </div>
            </div>
            <div className="space-y-2">
                {players.map((player) => (
                    <GameLobbyPlayerCard
                        key={player.playerId}
                        player={player}
                        isSelf={player.playerId === playerId}
                    />
                ))}
            </div>
        </>
    )
};