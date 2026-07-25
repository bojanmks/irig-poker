import type { Card } from "./Card";
import type { HandType } from "./HandType";
import type { Rank } from "./Rank";
import type { Suit } from "./Suit";

export interface RoundResolvedNotification {
    claimingPlayerId: string;
    claimedHand: HandType;
    ranks: Rank[];
    suit: Suit | null;
    callingPlayerId: string;
    wasTruthful: boolean;
    losingPlayerId: string;
    losingPlayerNewCardCount: number;
    allPlayerCards: { [key: string]: Card[] };
    eliminatedPlayerId: string | null;
    winnerPlayerId: string | null;
    winnerUsername: string | null;
}
