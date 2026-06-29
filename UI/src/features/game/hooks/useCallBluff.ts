import { useCallback } from "react";

import type { HubMethods } from "@/features/http/hooks/useHub";

export function useCallBluff(hub: HubMethods) {
    const callBluff = useCallback(async () => {
        await hub.invoke("CallBluff", {});
    }, [hub]);

    return { callBluff };
}
