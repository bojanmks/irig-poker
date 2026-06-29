import { useEffect } from "react";

import type { GameWonNotification } from "@/features/game/models/GameWonNotification";
import type { RoundResolvedNotification } from "@/features/game/models/RoundResolvedNotification";
import type { HubMethods } from "@/features/http/hooks/useHub";

import { useClaimEventHandlers } from "./useClaimEventHandlers";
import { useDealingEventHandlers } from "./useDealingEventHandlers";
import { useGameStartEventHandlers } from "./useGameStartEventHandlers";
import { useGameWonEventHandlers } from "./useGameWonEventHandlers";
import { usePlayerConnectionChangeEventHandlers } from "./usePlayerConnectionChangeEventHandlers";
import { useRoundEventHandlers } from "./useRoundEventHandlers";

type GameNotificationCallbacks = {
    onGameWon: (data: GameWonNotification) => void;
    onRoundResolved: (data: RoundResolvedNotification) => void;
};

export function useGamePageEventListeners(hub: HubMethods, callbacks: GameNotificationCallbacks) {
    const { connected: hubConnected, on: hubOn, off: hubOff } = hub;

    const { onPlayerLeft } = usePlayerConnectionChangeEventHandlers();
    const { onGameStarted } = useGameStartEventHandlers();
    const { onCardsDealt } = useDealingEventHandlers();
    const { onGameWon } = useGameWonEventHandlers(callbacks.onGameWon);
    const { onClaimMade, onGameStateUpdated } = useClaimEventHandlers();
    const { onRoundResolved } = useRoundEventHandlers(callbacks.onRoundResolved);

    useEffect(() => {
        if (hubConnected) {
            hubOn("PlayerLeft", onPlayerLeft);
            hubOn("GameStarted", onGameStarted);
            hubOn("CardsDealt", onCardsDealt);
            hubOn("GameWon", onGameWon);
            hubOn("ClaimMade", onClaimMade);
            hubOn("GameStateUpdated", onGameStateUpdated);
            hubOn("RoundResolved", onRoundResolved);
        }

        return () => {
            hubOff("PlayerLeft");
            hubOff("GameStarted");
            hubOff("CardsDealt");
            hubOff("GameWon");
            hubOff("ClaimMade");
            hubOff("GameStateUpdated");
            hubOff("RoundResolved");
        }
    }, [hubConnected, hubOn, hubOff, onPlayerLeft, onGameStarted, onCardsDealt, onGameWon, onClaimMade, onGameStateUpdated, onRoundResolved]);
}
