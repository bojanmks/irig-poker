export const HandType = {
    HighCard: 0,
    OnePair: 1,
    TwoPair: 2,
    Straight: 3,
    ThreeOfAKind: 4,
    FullHouse: 5,
    FourOfAKind: 6,
    StraightFlush: 7
} as const;

export type HandType = (typeof HandType)[keyof typeof HandType];