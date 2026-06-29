import { useCallback } from "react";

import type { PublicGameState } from "@/features/game/models/PublicGameState";
import { claimMade, gameStateUpdated } from "@/features/game/store/gameStateSlice";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { useAppDispatch } from "@/features/store/hooks";

export function useClaimEventHandlers() {
    const dispatch = useAppDispatch();

    const onClaimMade = useCallback(() => {
        dispatch(claimMade());
    }, [dispatch]);

    const onGameStateUpdated = useCallback((notification: HubNotification<PublicGameState>) => {
        dispatch(gameStateUpdated(notification.data));
    }, [dispatch]);

    return { onClaimMade, onGameStateUpdated };
}
