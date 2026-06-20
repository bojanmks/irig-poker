import connection from "@/features/http/clients/hubClient";
import { useMemo } from "react";
import { useAppSelector } from "@/features/store/hooks";

export function usePlayerInfo() {
    const gameState = useAppSelector((state) => state.gameState.gameState);

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