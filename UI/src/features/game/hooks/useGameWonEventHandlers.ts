import { useCallback } from "react";

import playerWonSound from "@/assets/player_won.mp3";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { playSound } from "@/features/shared/utils/audio";

import type { GameWonNotification } from "../models/GameWonNotification";

export function useGameWonEventHandlers(onGameWon: (data: GameWonNotification) => void) {
    const onGameWonHandler = useCallback((notification: HubNotification<GameWonNotification>) => {
        playSound(playerWonSound);
        onGameWon(notification.data);
    }, [onGameWon]);

    return { onGameWon: onGameWonHandler };
}
