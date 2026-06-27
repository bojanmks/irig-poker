import { useMemo } from "react";

import { useAppSelector } from "@/features/store/hooks";

import { GamePlayerCard } from "./GamePlayerCard";

export const GamePlayerList = () => {
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

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-3">
            {players.map((player) => (
                <GamePlayerCard
                    key={player.playerId}
                    player={player}
                    isCurrentTurn={player.playerId === gameState?.currentTurnPlayerId}
                    isSelf={player.playerId === playerId}
                />
            ))}
        </div>
    )
};
