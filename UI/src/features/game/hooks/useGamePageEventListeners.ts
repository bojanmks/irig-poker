import { useEffect } from "react";

import type { HubMethods } from "@/features/http/hooks/useHub";

import { useClaimEventHandlers } from "./useClaimEventHandlers";
import { useGameStartEventHandlers } from "./useGameStartEventHandlers";
import { useGameWonEventHandlers } from "./useGameWonEventHandlers";
import { usePlayerConnectionChangeEventHandlers } from "./usePlayerConnectionChangeEventHandlers";
import { useRoundEventHandlers } from "./useRoundEventHandlers";

export function useGamePageEventListeners(hub: HubMethods) {
    const { connected: hubConnected, on: hubOn, off: hubOff } = hub;

    const { onPlayerJoined, onPlayerLeft, onAdminChanged } = usePlayerConnectionChangeEventHandlers();
    const { onGameStarted, onCardsDealt } = useGameStartEventHandlers();
    const { onGameWon } = useGameWonEventHandlers();
    const { onClaimMade, onGameStateUpdated } = useClaimEventHandlers();
    const { onRoundResolved, onPlayerEliminated } = useRoundEventHandlers();

    useEffect(() => {
        if (hubConnected) {
            hubOn("PlayerJoined", onPlayerJoined);
            hubOn("PlayerLeft", onPlayerLeft);
            hubOn("AdminChanged", onAdminChanged);
            hubOn("GameStarted", onGameStarted);
            hubOn("CardsDealt", onCardsDealt);
            hubOn("GameWon", onGameWon);
            hubOn("ClaimMade", onClaimMade);
            hubOn("GameStateUpdated", onGameStateUpdated);
            hubOn("RoundResolved", onRoundResolved);
            hubOn("PlayerEliminated", onPlayerEliminated);
        }

        return () => {
            hubOff("PlayerJoined");
            hubOff("PlayerLeft");
            hubOff("AdminChanged");
            hubOff("GameStarted");
            hubOff("CardsDealt");
            hubOff("GameWon");
            hubOff("ClaimMade");
            hubOff("GameStateUpdated");
            hubOff("RoundResolved");
            hubOff("PlayerEliminated");
        }
    }, [hubConnected, hubOn, hubOff, onPlayerJoined, onPlayerLeft, onAdminChanged, onGameStarted, onCardsDealt, onGameWon, onClaimMade, onGameStateUpdated, onRoundResolved, onPlayerEliminated]);
}
