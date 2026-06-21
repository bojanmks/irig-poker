import { useEffect } from "react";

import type { HubMethods } from "@/features/http/hooks/useHub";

import { useGameStartListeners } from "./useGameStartListeners";
import { usePlayerConnectionChangeListeners } from "./usePlayerConnectionChangeListeners";

export function useGamePageEventListeners(hub: HubMethods) {
    const { connected: hubConnected, on: hubOn, off: hubOff } = hub;

    const { onPlayerJoined, onPlayerLeft } = usePlayerConnectionChangeListeners();
    const { onGameStarted } = useGameStartListeners();

    useEffect(() => {
        if (hubConnected) {
            hubOn("PlayerJoined", onPlayerJoined);
            hubOn("PlayerLeft", onPlayerLeft);
            hubOn("GameStarted", onGameStarted);
        }

        return () => {
            hubOff("PlayerJoined");
            hubOff("PlayerLeft");
            hubOff("GameStarted");
        }
    }, [hubConnected, hubOn, hubOff, onPlayerJoined, onPlayerLeft, onGameStarted]);
}