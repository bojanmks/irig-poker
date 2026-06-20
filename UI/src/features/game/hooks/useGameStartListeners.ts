import { useCallback } from "react";

import { gameStarted } from "@/features/game/store/gameStateSlice";
import { useAppDispatch } from "@/features/store/hooks";

export function useGameStartListeners() {
    const dispatch = useAppDispatch();

    const onGameStarted = useCallback(() => {
        dispatch(gameStarted());
    }, [dispatch]);

    return { onGameStarted };
}