import { useCallback } from "react";
import { useAppDispatch } from "@/features/store/hooks";
import { gameStarted } from "@/features/game/store/gameStateSlice";

export function useGameStartListeners() {
    const dispatch = useAppDispatch();

    const onGameStarted = useCallback(() => {
        dispatch(gameStarted());
    }, [dispatch]);

    return { onGameStarted };
}