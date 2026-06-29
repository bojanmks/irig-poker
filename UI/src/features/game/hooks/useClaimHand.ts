import { useCallback } from "react";

import type { HubMethods } from "@/features/http/hooks/useHub";

import type { HandType } from "../models/HandType";

export function useClaimHand(hub: HubMethods) {
    const claimHand = useCallback(async (claimedHand: HandType) => {
        await hub.invoke("ClaimHand", { claimedHand });
    }, [hub]);

    return { claimHand };
}
