import type { HandType } from "./HandType";

export interface ClaimNotification {
    claimingPlayerId: string;
    claimedHand: HandType;
    ranks: number[];
}
