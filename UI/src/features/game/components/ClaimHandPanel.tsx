import { useCallback } from "react";
import { useTranslation } from "react-i18next";

import type { HubMethods } from "@/features/http/hooks/useHub";
import { useAppSelector } from "@/features/store/hooks";

import { useCallBluff } from "../hooks/useCallBluff";
import { HandType } from "../models/HandType";
import type { Rank } from "../models/Rank";
import { Suit } from "../models/Suit";

import { HandSelector } from "./HandSelector";

const handTypeLabels: Record<HandType, string> = {
    [HandType.HighCard]: "game.hands.highCard",
    [HandType.OnePair]: "game.hands.onePair",
    [HandType.TwoPair]: "game.hands.twoPair",
    [HandType.Straight]: "game.hands.straight",
    [HandType.ThreeOfAKind]: "game.hands.threeOfAKind",
    [HandType.FullHouse]: "game.hands.fullHouse",
    [HandType.FourOfAKind]: "game.hands.fourOfAKind",
    [HandType.StraightFlush]: "game.hands.straightFlush",
};

const suitSymbols: Record<number, string> = {
    [Suit.Hearts]: "♥",
    [Suit.Diamonds]: "♦",
    [Suit.Clubs]: "♣",
    [Suit.Spades]: "♠",
};

function rankLabel(rank: number): string {
    switch (rank) {
        case 2: return "2"; case 3: return "3"; case 4: return "4";
        case 5: return "5"; case 6: return "6"; case 7: return "7";
        case 8: return "8"; case 9: return "9"; case 10: return "10";
        case 12: return "J"; case 13: return "Q"; case 14: return "K";
        case 99: return "A"; default: return "?";
    }
}

function describeRanks(handType: HandType, ranks: number[], suit?: number | null): string {
    if (ranks.length === 0) return "";
    if (handType === HandType.TwoPair) {
        return `${rankLabel(ranks[0])} & ${rankLabel(ranks[1])}`;
    }
    if (handType === HandType.FullHouse) {
        return `${rankLabel(ranks[0])} over ${rankLabel(ranks[1])}`;
    }
    const label = rankLabel(ranks[0]);
    if (handType === HandType.StraightFlush && suit !== null && suit !== undefined) {
        return `${label}${suitSymbols[suit] ?? ""}`;
    }
    return label;
}

type ClaimHandPanelProps = {
    hub: HubMethods;
};

export const ClaimHandPanel = ({ hub }: ClaimHandPanelProps) => {
    const { t } = useTranslation();
    const gameState = useAppSelector((state) => state.gameState.gameState);
    const playerId = useAppSelector((state) => state.gameState.playerId);
    const { callBluff } = useCallBluff(hub);

    const handleClaim = useCallback(async (handType: HandType, ranks: Rank[], suit?: number) => {
        await hub.invoke("ClaimHand", { claimedHand: handType, ranks, suit });
    }, [hub]);

    if (!gameState) {
        return null;
    }

    const isMyTurn = gameState.currentTurnPlayerId === playerId;
    const currentClaim = gameState.currentClaimedHand;
    const currentClaimerId = gameState.claimingPlayerId;
    const currentClaimer = currentClaimerId ? gameState.players[currentClaimerId] : null;
    const currentRanks = gameState.ranks;

    return (
        <div className="flex flex-col gap-3 p-4 rounded-lg border border-border bg-card/50">
            {currentClaimer && currentClaim !== null && currentRanks !== null && (
                <div className="text-sm text-muted-foreground">
                    <span className="font-semibold text-foreground">{currentClaimer.username}</span>
                    {" "}{t("game.claimed")}{" "}
                    <span className="font-semibold text-foreground">{t(handTypeLabels[currentClaim])}</span>
                    {" "}({describeRanks(currentClaim, currentRanks, gameState.claimedSuit)})
                </div>
            )}

            {isMyTurn && (
                <div className="flex flex-col gap-2">
                    {currentClaim !== null && (
                        <button
                            onClick={callBluff}
                            className="px-4 py-2 text-sm font-medium rounded-md bg-destructive text-destructive-foreground hover:bg-destructive/90 transition-colors"
                        >
                            {t("game.callBluff")}
                        </button>
                    )}
                    <HandSelector
                        onSelect={handleClaim}
                        currentClaimedHand={currentClaim}
                        currentRanks={currentRanks}
                    />
                </div>
            )}

            {!isMyTurn && currentClaim === null && (
                <p className="text-sm text-muted-foreground">{t("game.waitingForFirstClaim")}</p>
            )}

            {!isMyTurn && currentClaim !== null && (
                <p className="text-sm text-muted-foreground">{t("game.waitingForChallenge")}</p>
            )}
        </div>
    );
};
