import type { Player } from "./Player";

export interface PublicGameState {
    hasStarted: boolean;
    players: { [key: string]: Player; };
}