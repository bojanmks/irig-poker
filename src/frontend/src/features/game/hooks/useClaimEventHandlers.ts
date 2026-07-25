import { useCallback } from "react";

import claimMadeSound from "@/assets/claim_made.mp3";
import type { PublicGameState } from "@/features/game/models/PublicGameState";
import { gameStateUpdated } from "@/features/game/store/gameStateSlice";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { playSound } from "@/features/shared/utils/audio";
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";

import type { ClaimNotification } from "../models/ClaimNotification";

export function useClaimEventHandlers() {
    const dispatch = useAppDispatch();
    const playerId = useAppSelector((state) => state.gameState.playerId);

    const onClaimMade = useCallback((notification: HubNotification<ClaimNotification>) => {
        if (playerId !== notification.data.claimingPlayerId) {
            playSound(claimMadeSound);
        }
    }, [playerId]);

    const onGameStateUpdated = useCallback((notification: HubNotification<PublicGameState>) => {
        dispatch(gameStateUpdated(notification.data));
    }, [dispatch]);

    return { onClaimMade, onGameStateUpdated };
}
