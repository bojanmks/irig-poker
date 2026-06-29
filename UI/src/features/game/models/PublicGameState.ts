import type { HandType } from "./HandType";
import type { Player } from "./Player";
import type { Rank } from "./Rank";

export interface PublicGameState {
    hasStarted: boolean;
    players: { [key: string]: Player; };
    playerOrder: string[];
    currentTurnPlayerId: string | null;
    currentClaimedHand: HandType | null;
    claimingPlayerId: string | null;
    ranks: Rank[] | null;
}
