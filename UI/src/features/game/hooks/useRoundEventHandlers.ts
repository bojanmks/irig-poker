import { useCallback } from "react";

import type { RoundResolvedNotification } from "@/features/game/models/RoundResolvedNotification";
import { playerEliminated, roundResolved } from "@/features/game/store/gameStateSlice";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { useAppDispatch } from "@/features/store/hooks";

export function useRoundEventHandlers() {
    const dispatch = useAppDispatch();

    const onRoundResolved = useCallback((notification: HubNotification<RoundResolvedNotification>) => {
        dispatch(roundResolved(notification.data));
    }, [dispatch]);

    const onPlayerEliminated = useCallback((notification: HubNotification<string>) => {
        dispatch(playerEliminated(notification.data));
    }, [dispatch]);

    return { onRoundResolved, onPlayerEliminated };
}
