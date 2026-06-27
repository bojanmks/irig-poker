import { useEffect } from "react";

import type { HubMethods } from "@/features/http/hooks/useHub";

import { useGameStartEventHandlers } from "./useGameStartEventHandlers";
import { useGameWonEventHandlers } from "./useGameWonEventHandlers";
import { usePlayerConnectionChangeEventHandlers } from "./usePlayerConnectionChangeEventHandlers";

export function useGamePageEventListeners(hub: HubMethods) {
    const { connected: hubConnected, on: hubOn, off: hubOff } = hub;

    const { onPlayerJoined, onPlayerLeft, onAdminChanged } = usePlayerConnectionChangeEventHandlers();
    const { onGameStarted } = useGameStartEventHandlers();
    const { onGameWon } = useGameWonEventHandlers();

    useEffect(() => {
        if (hubConnected) {
            hubOn("PlayerJoined", onPlayerJoined);
            hubOn("PlayerLeft", onPlayerLeft);
            hubOn("AdminChanged", onAdminChanged);
            hubOn("GameStarted", onGameStarted);
            hubOn("GameWon", onGameWon);
        }

        return () => {
            hubOff("PlayerJoined");
            hubOff("PlayerLeft");
            hubOff("AdminChanged");
            hubOff("GameStarted");
            hubOff("GameWon");
        }
    }, [hubConnected, hubOn, hubOff, onPlayerJoined, onPlayerLeft, onAdminChanged, onGameStarted, onGameWon]);
}