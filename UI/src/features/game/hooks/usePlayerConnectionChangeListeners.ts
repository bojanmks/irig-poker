import type { Player } from "../models/Player";
import { useGameState } from "../contexts/GameStateContext";
import { useCallback } from "react";
import type { HubNotification } from "@/features/http/models/HubNotification";

export function usePlayerConnectionChangeListeners() {
    const { setGameState } = useGameState();

    const onPlayerJoined = useCallback((notification: HubNotification<{ connectionId: string, player: Player }>) => {
        setGameState(currentGameState => {
            if (!currentGameState) {
                return currentGameState;
            }

            return {
                ...currentGameState,
                players: {
                    ...currentGameState.players,
                    [notification.data.connectionId]: notification.data.player
                }
            };
        });
    }, [setGameState]);

    const onPlayerLeft = useCallback((notification: HubNotification<string>) => {
        setGameState(currentGameState => {
            if (!currentGameState) {
                return currentGameState;
            }
            
            const { [notification.data]: removedPlayer, ...remainingPlayers } = currentGameState.players;

            return {
                ...currentGameState,
                players: remainingPlayers
            };
        });
    }, [setGameState]);

    return {
        onPlayerJoined,
        onPlayerLeft
    }
}