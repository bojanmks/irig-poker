import type { HandType } from "./HandType";
import type { Suit } from "./Suit";

export interface ClaimNotification {
    claimingPlayerId: string;
    claimedHand: HandType;
    ranks: number[];
    suit: Suit | null;
}
