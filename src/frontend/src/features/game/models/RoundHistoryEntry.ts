import type { HandType } from "./HandType";
import type { Rank } from "./Rank";
import type { Suit } from "./Suit";

export interface RoundHistoryEntry {
    claimingPlayerId: string;
    claimedHand: HandType;
    ranks: Rank[];
    suit: Suit | null;
}
