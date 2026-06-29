import { useEffect } from "react";

import { clearHangingNotifications } from "@/features/http/util/hangingNotifications";

export function useDiconnectOnPageLeave (disconnect: () => void) {
    useEffect(() => {
        return () => {
            clearHangingNotifications();
            disconnect();
        }
    }, [disconnect]);
}