import { useGameState } from "../contexts/GameStateContext";
import { useCallback } from "react";

export function useGameStartListeners() {
    const { setGameState } = useGameState();

    const onGameStarted = useCallback(() => {
        setGameState(oldState => {
            if (!oldState) {
                return oldState;
            }

            return {
                ...oldState,
                hasStarted: true
            };
        });
    }, [setGameState]);

    return { onGameStarted };
}