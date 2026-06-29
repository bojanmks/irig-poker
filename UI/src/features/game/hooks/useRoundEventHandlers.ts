import { useCallback } from "react";

import type { RoundResolvedNotification } from "@/features/game/models/RoundResolvedNotification";
import type { HubNotification } from "@/features/http/models/HubNotification";

export function useRoundEventHandlers(onRoundResolvedCallback: (data: RoundResolvedNotification) => void) {
    const onRoundResolved = useCallback((notification: HubNotification<RoundResolvedNotification>) => {
        onRoundResolvedCallback(notification.data);
    }, [onRoundResolvedCallback]);

    return { onRoundResolved };
}
