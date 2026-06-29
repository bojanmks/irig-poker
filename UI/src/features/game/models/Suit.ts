export const Suit = {
    Hearts: 0,
    Diamonds: 1,
    Clubs: 2,
    Spades: 3
} as const;

export type Suit = (typeof Suit)[keyof typeof Suit];