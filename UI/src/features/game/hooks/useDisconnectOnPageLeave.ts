import { clearHangingNotifications } from "@/features/http/hooks/useHangingNotifications";
import { useEffect } from "react";

export function useDiconnectOnPageLeave (disconnect: () => void) {
    useEffect(() => {
        return () => {
            clearHangingNotifications();
            disconnect();
        }
    }, [disconnect]);
}