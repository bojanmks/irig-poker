import { useCallback } from "react";

import { playerJoined, playerLeft } from "@/features/game/store/gameStateSlice";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { useAppDispatch } from "@/features/store/hooks";

import type { Player } from "../models/Player";

export function usePlayerConnectionChangeListeners() {
    const dispatch = useAppDispatch();

    const onPlayerJoined = useCallback((notification: HubNotification<{ playerId: string, player: Player }>) => {
        dispatch(playerJoined({ playerId: notification.data.playerId, player: notification.data.player }));
    }, [dispatch]);

    const onPlayerLeft = useCallback((notification: HubNotification<string>) => {
        dispatch(playerLeft(notification.data));
    }, [dispatch]);

    return {
        onPlayerJoined,
        onPlayerLeft
    }
}