import connection from "@/features/http/clients/hubClient";
import { useGameState } from "../contexts/GameStateContext";
import { useMemo } from "react";

export function usePlayerInfo() {
    const { gameState } = useGameState();

    const isAdmin = useMemo(() => {
        if (!connection?.connectionId) {
            return false;
        }

        return gameState?.players[connection.connectionId]?.isAdmin ?? false;
    }, [gameState?.players]);

    return {
        isAdmin
    }
}