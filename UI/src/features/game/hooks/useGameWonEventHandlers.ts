import { useCallback } from "react";

import type { HubNotification } from "@/features/http/models/HubNotification";

import type { GameWonNotification } from "../models/GameWonNotification";

export function useGameWonEventHandlers(onGameWon: (data: GameWonNotification) => void) {
    const onGameWonHandler = useCallback((notification: HubNotification<GameWonNotification>) => {
        onGameWon(notification.data);
    }, [onGameWon]);

    return { onGameWon: onGameWonHandler };
}
