import { useEffect } from "react";

import { clearHangingNotifications } from "@/features/http/hooks/useHangingNotifications";

export function useDiconnectOnPageLeave (disconnect: () => void) {
    useEffect(() => {
        return () => {
            clearHangingNotifications();
            disconnect();
        }
    }, [disconnect]);
}