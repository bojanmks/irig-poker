import { HandType } from "../models/HandType";
import { Rank } from "../models/Rank";
import { Suit } from "../models/Suit";

import { ClaimedHandCards } from "./ClaimedHandCards";

type HandTypePreviewProps = {
    handType: HandType;
    cardWidth?: number;
    className?: string;
};

const PREVIEW_SUIT = Suit.Spades;

// Representative ranks, since no specific ranks have been picked yet.
const PREVIEW_RANKS: Record<HandType, Rank[]> = {
    [HandType.HighCard]: [Rank.Ace],
    [HandType.OnePair]: [Rank.Queen],
    [HandType.TwoPair]: [Rank.King, Rank.Seven],
    [HandType.Straight]: [Rank.Seven],
    [HandType.ThreeOfAKind]: [Rank.Jack],
    [HandType.FullHouse]: [Rank.King, Rank.Seven],
    [HandType.FourOfAKind]: [Rank.Queen],
    [HandType.StraightFlush]: [Rank.Seven]
};

export const HandTypePreview = ({ handType, cardWidth = 48, className }: HandTypePreviewProps) => {
    return (
        <div
            aria-hidden="true"
            className={className}
        >
            <ClaimedHandCards
                handType={handType}
                ranks={PREVIEW_RANKS[handType]}
                suit={PREVIEW_SUIT}
                displayWidth={cardWidth}
            />
        </div>
    );
};
