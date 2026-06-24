import { useCallback } from "react";

import { adminChanged, playerJoined, playerLeft } from "@/features/game/store/gameStateSlice";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { useAppDispatch } from "@/features/store/hooks";

import type { Player } from "../models/Player";

export function usePlayerConnectionChangeEventHandlers() {
    const dispatch = useAppDispatch();

    const onPlayerJoined = useCallback((notification: HubNotification<{ player: Player, playerOrder: string[] }>) => {
        dispatch(playerJoined({ player: notification.data.player, playerOrder: notification.data.playerOrder }));
    }, [dispatch]);

    const onPlayerLeft = useCallback((notification: HubNotification<string>) => {
        dispatch(playerLeft(notification.data));
    }, [dispatch]);

    const onAdminChanged = useCallback((notification: HubNotification<string>) => {
        dispatch(adminChanged(notification.data));
    }, [dispatch]);

    return {
        onPlayerJoined,
        onPlayerLeft,
        onAdminChanged,
    }
}