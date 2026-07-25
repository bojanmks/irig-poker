import { useMemo } from "react";

import { useAppSelector } from "@/features/store/hooks";

export function usePlayerInfo() {
    const gameState = useAppSelector((state) => state.gameState.gameState);
    const playerId = useAppSelector((state) => state.gameState.playerId);

    const isAdmin = useMemo(() => {
        if (!playerId) {
            return false;
        }

        return gameState?.players[playerId]?.isAdmin ?? false;
    }, [gameState?.players, playerId]);

    return {
        isAdmin
    }
}