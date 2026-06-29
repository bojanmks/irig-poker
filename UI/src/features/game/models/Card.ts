import type { Rank } from "./Rank";
import type { Suit } from "./Suit";

export interface Card {
    suit: Suit;
    rank: Rank;
}
