import { useCallback, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";

import { cn } from "@/lib/utils";

import { HandType } from "../models/HandType";
import { Rank } from "../models/Rank";
import { Suit } from "../models/Suit";

import { CardSprite } from "./CardSprite";

const handTypes = [
    HandType.HighCard,
    HandType.OnePair,
    HandType.TwoPair,
    HandType.Straight,
    HandType.ThreeOfAKind,
    HandType.FullHouse,
    HandType.FourOfAKind,
    HandType.StraightFlush,
];

const allRanks: Rank[] = [
    Rank.Two,
    Rank.Three,
    Rank.Four,
    Rank.Five,
    Rank.Six,
    Rank.Seven,
    Rank.Eight,
    Rank.Nine,
    Rank.Ten,
    Rank.Jack,
    Rank.Queen,
    Rank.King,
    Rank.Ace
];

function straightValue(rank: Rank): number {
    switch (rank) {
        case 12: return 11;
        case 13: return 12;
        case 14: return 13;
        case 99: return 14;
        default: return rank;
    }
}

function isStraightHandType(ht: HandType): boolean {
    return ht === HandType.Straight || ht === HandType.StraightFlush;
}

function isStrongerThan(handType: HandType, ranks: Rank[], otherHandType: HandType, otherRanks: Rank[]): boolean {
    if (handType !== otherHandType) return handType > otherHandType;
    for (let i = 0; i < Math.min(ranks.length, otherRanks.length); i++) {
        const cmp = straightValue(ranks[i]) - straightValue(otherRanks[i]);
        if (cmp !== 0) return cmp > 0;
    }
    return ranks.length > otherRanks.length;
}

function handTypeLabelKey(handType: HandType): string {
    switch (handType) {
        case HandType.HighCard: return "game.hands.highCard";
        case HandType.OnePair: return "game.hands.onePair";
        case HandType.TwoPair: return "game.hands.twoPair";
        case HandType.Straight: return "game.hands.straight";
        case HandType.ThreeOfAKind: return "game.hands.threeOfAKind";
        case HandType.FullHouse: return "game.hands.fullHouse";
        case HandType.FourOfAKind: return "game.hands.fourOfAKind";
        case HandType.StraightFlush: return "game.hands.straightFlush";
    }
}

type Step = "hand-type" | "suit" | "first-rank" | "second-rank";

type HandSelectorProps = {
    onSelect: (handType: HandType, ranks: Rank[]) => void;
    currentClaimedHand: HandType | null;
    currentRanks: Rank[] | null;
    disabled?: boolean;
};

export const HandSelector = ({ onSelect, currentClaimedHand, currentRanks, disabled }: HandSelectorProps) => {
    const { t } = useTranslation();
    const [step, setStep] = useState<Step>("hand-type");
    const [selectedHandType, setSelectedHandType] = useState<HandType | null>(null);
    const [selectedSuit, setSelectedSuit] = useState<Suit>(Suit.Hearts);
    const [firstRank, setFirstRank] = useState<Rank | null>(null);

    const handleHandTypeClick = useCallback((ht: HandType) => {
        setSelectedHandType(ht);
        if (ht === HandType.StraightFlush) {
            setStep("suit");
        } else {
            setStep("first-rank");
        }
        setFirstRank(null);
    }, [setStep, setFirstRank]);

    const handleSuitClick = useCallback((suit: Suit) => {
        setSelectedSuit(suit);
        setStep("first-rank");
    }, [setSelectedSuit, setStep]);

    const handleFirstRankClick = useCallback((rank: Rank) => {
        if (selectedHandType === null) return;

        if (selectedHandType === HandType.TwoPair || selectedHandType === HandType.FullHouse) {
            setFirstRank(rank);
            setStep("second-rank");
        } else {
            onSelect(selectedHandType, [rank]);
            setStep("hand-type");
            setSelectedHandType(null);
        }
    }, [selectedHandType, onSelect]);

    const handleSecondRankClick = useCallback((rank: Rank) => {
        if (selectedHandType === null || firstRank === null) return;
        onSelect(selectedHandType, [firstRank, rank]);
        setStep("hand-type");
        setSelectedHandType(null);
        setFirstRank(null);
    }, [selectedHandType, firstRank, onSelect]);

    const handleBack = useCallback(() => {
        if (step === "second-rank") {
            setStep("first-rank");
            setFirstRank(null);
        } else if (step === "suit" || selectedHandType !== HandType.StraightFlush) {
            setStep("hand-type");
            setSelectedHandType(null);
        } else {
            setStep("suit");
        }
    }, [step, selectedHandType]);

    const displayedRanks = useMemo<Rank[]>(() => {
        if (selectedHandType !== null && isStraightHandType(selectedHandType)) {
            return [Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace];
        }

        return allRanks;
    }, [selectedHandType]);

    const isHandTypeEnabled = useCallback((ht: HandType) => {
        if (disabled) return false;
        if (currentClaimedHand === null || currentRanks === null) return true;
        return ht >= currentClaimedHand;
    }, [disabled, currentClaimedHand, currentRanks]);

    const isRankEnabled = useCallback((rank: Rank) => {
        if (selectedHandType === null) return false;
        if (currentClaimedHand === null || currentRanks === null) return true;

        if (step === "first-rank") {
            if (selectedHandType === HandType.TwoPair || selectedHandType === HandType.FullHouse) {
                return straightValue(rank) >= straightValue(currentRanks[0]);
            }
            const testRanks: Rank[] = [rank];
            return isStrongerThan(selectedHandType, testRanks, currentClaimedHand, currentRanks);
        }

        if (step === "second-rank" && firstRank !== null) {
            const testRanks: Rank[] = [firstRank, rank];
            return isStrongerThan(selectedHandType, testRanks, currentClaimedHand, currentRanks);
        }

        return true;
    }, [selectedHandType, currentClaimedHand, currentRanks, step, firstRank]);

    if (step === "hand-type") {
        return (
            <div className="flex flex-col gap-2">
                <h3 className="text-sm font-semibold text-muted-foreground uppercase tracking-wide">
                    {t("game.claimHand")}
                </h3>
                <div className="flex flex-wrap gap-2">
                    {handTypes.map(ht => (
                        <button
                            key={ht}
                            onClick={() => handleHandTypeClick(ht)}
                            disabled={!isHandTypeEnabled(ht)}
                            className="px-3 py-1.5 text-sm font-medium rounded-md border border-border bg-card hover:bg-accent hover:text-accent-foreground transition-colors disabled:opacity-50 disabled:pointer-events-none"
                        >
                            {t(handTypeLabelKey(ht))}
                        </button>
                    ))}
                </div>
            </div>
        );
    }

    if (step === "suit") {
        const suits: Suit[] = [Suit.Hearts, Suit.Diamonds, Suit.Clubs, Suit.Spades];
        return (
            <div className="flex flex-col gap-2">
                <div className="flex items-center gap-2">
                    <button
                        onClick={handleBack}
                        className="text-sm text-muted-foreground hover:text-foreground transition-colors"
                    >
                        &larr; {t("game.back")}
                    </button>
                    <h3 className="text-sm font-semibold text-muted-foreground uppercase tracking-wide">
                        {t(handTypeLabelKey(selectedHandType!))}
                        {" — "}{t("game.selectSuit")}
                    </h3>
                </div>
                <div className="flex gap-2 justify-center">
                    {suits.map(suit => (
                        <button
                            key={suit}
                            onClick={() => handleSuitClick(suit)}
                            className="rounded-md transition-all hover:ring-2 hover:ring-primary hover:ring-offset-2"
                        >
                            <CardSprite suit={suit} rank={99} displayWidth={60} />
                        </button>
                    ))}
                </div>
            </div>
        );
    }

    return (
        <div className="flex flex-col gap-2">
            <div className="flex items-center gap-2">
                <button
                    onClick={handleBack}
                    className="text-sm text-muted-foreground hover:text-foreground transition-colors"
                >
                    &larr; {t("game.back")}
                </button>
                <h3 className="text-sm font-semibold text-muted-foreground uppercase tracking-wide">
                    {t(handTypeLabelKey(selectedHandType!))}
                    {step === "second-rank" && t("game.selectSecondRank")}
                </h3>
            </div>
            <div className="flex flex-wrap gap-1 justify-center">
                {displayedRanks.map(rank => {
                    const enabled = isRankEnabled(rank);
                    const selected = rank === (step === "second-rank" ? firstRank : null);
                    return (
                        <button
                            key={rank}
                            onClick={() => step === "second-rank" ? handleSecondRankClick(rank) : handleFirstRankClick(rank)}
                            disabled={!enabled || selected}
                            className={cn(
                                "rounded-xs transition-all",
                                "disabled:opacity-40 disabled:pointer-events-none",
                                (!enabled || selected) ? "grayscale" : "hover:ring-2 hover:ring-primary hover:ring-offset-1"
                            )}
                        >
                            <CardSprite suit={selectedSuit} rank={rank} displayWidth={50} />
                        </button>
                    );
                })}
            </div>
        </div>
    );
};
