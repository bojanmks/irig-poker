import type { Card } from "./Card";
import type { Player } from "./Player";

export interface PublicGameState {
    hasStarted: boolean;
    players: { [key: string]: Player; };
    playerOrder: string[];
    playerCards: { [key: string]: Card[] };
    currentTurnPlayerId: string | null;
}