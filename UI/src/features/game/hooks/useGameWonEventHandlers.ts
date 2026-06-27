import { useCallback } from "react";

import type { HubNotification } from "@/features/http/models/HubNotification";
import { useAppDispatch } from "@/features/store/hooks";

import type { GameWonNotification } from "../models/GameWonNotification";
import { gameWon } from "../store/gameStateSlice";

export function useGameWonEventHandlers() {
    const dispatch = useAppDispatch();

    const onGameWon = useCallback((notification: HubNotification<GameWonNotification>) => {
        dispatch(gameWon(notification.data));
    }, [dispatch]);

    return { onGameWon };
}
