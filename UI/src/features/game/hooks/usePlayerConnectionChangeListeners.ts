import { useCallback } from "react";

import { playerJoined, playerLeft } from "@/features/game/store/gameStateSlice";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { useAppDispatch } from "@/features/store/hooks";

import type { Player } from "../models/Player";

export function usePlayerConnectionChangeListeners() {
    const dispatch = useAppDispatch();

    const onPlayerJoined = useCallback((notification: HubNotification<{ connectionId: string, player: Player }>) => {
        dispatch(playerJoined({ connectionId: notification.data.connectionId, player: notification.data.player }));
    }, [dispatch]);

    const onPlayerLeft = useCallback((notification: HubNotification<string>) => {
        dispatch(playerLeft(notification.data));
    }, [dispatch]);

    return {
        onPlayerJoined,
        onPlayerLeft
    }
}