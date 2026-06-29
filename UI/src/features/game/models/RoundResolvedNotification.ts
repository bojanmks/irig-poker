import type { Card } from "./Card";
import type { HandType } from "./HandType";

export interface RoundResolvedNotification {
    claimingPlayerId: string;
    claimedHand: HandType;
    ranks: number[];
    callingPlayerId: string;
    wasTruthful: boolean;
    losingPlayerId: string;
    losingPlayerNewCardCount: number;
    allPlayerCards: { [key: string]: Card[] };
    eliminatedPlayerId: string | null;
    winnerPlayerId: string | null;
    winnerUsername: string | null;
}
