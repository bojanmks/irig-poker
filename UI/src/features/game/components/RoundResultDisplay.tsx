import { useTranslation } from "react-i18next";

import { useAppSelector } from "@/features/store/hooks";

import { HandType } from "../models/HandType";

import { CardSprite } from "./CardSprite";

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

function rankLabel(rank: number): string {
    switch (rank) {
        case 2: return "2"; case 3: return "3"; case 4: return "4";
        case 5: return "5"; case 6: return "6"; case 7: return "7";
        case 8: return "8"; case 9: return "9"; case 10: return "10";
        case 12: return "J"; case 13: return "Q"; case 14: return "K";
        case 99: return "A"; default: return "?";
    }
}

function describeRanks(handType: HandType, ranks: number[]): string {
    if (ranks.length === 0) return "";
    if (handType === HandType.TwoPair) {
        return `${rankLabel(ranks[0])} & ${rankLabel(ranks[1])}`;
    }
    if (handType === HandType.FullHouse) {
        return `${rankLabel(ranks[0])} over ${rankLabel(ranks[1])}`;
    }
    return rankLabel(ranks[0]);
}

export const RoundResultDisplay = () => {
    const { t } = useTranslation();
    const roundResult = useAppSelector((state) => state.gameState.roundResult);
    const gameState = useAppSelector((state) => state.gameState.gameState);

    if (!roundResult || !gameState) {
        return null;
    }

    const loserUsername = gameState.players[roundResult.losingPlayerId]?.username ?? "?";
    const hasWinner = roundResult.winnerPlayerId !== null;
    const winnerUsername = roundResult.winnerUsername ?? "";

    return (
        <div className="flex flex-col gap-4 p-4 rounded-lg border border-border bg-card">
            <div className="text-center">
                <p className="text-sm text-muted-foreground">
                    <span className="font-semibold text-foreground">
                        {gameState.players[roundResult.claimingPlayerId]?.username}
                    </span>
                    {" "}{t("game.claimed")}{" "}
                    <span className="font-semibold text-foreground">
                        {t(handTypeLabels[roundResult.claimedHand])}
                    </span>
                    {" "}({describeRanks(roundResult.claimedHand, roundResult.ranks)})
                </p>
                <p className="text-sm text-muted-foreground">
                    <span className="font-semibold text-foreground">
                        {gameState.players[roundResult.callingPlayerId]?.username}
                    </span>
                    {" "}{t("game.calledBluff")}
                </p>
            </div>

            <div className="text-center">
                {roundResult.wasTruthful ? (
                    <p className="text-sm font-semibold text-green-600 dark:text-green-400">
                        {t("game.claimWasTruthful")}
                    </p>
                ) : (
                    <p className="text-sm font-semibold text-red-600 dark:text-red-400">
                        {t("game.claimWasBluff")}
                    </p>
                )}
            </div>

            <div className="text-center">
                <p className="text-sm">
                    {t("game.loserGetsCard", { username: loserUsername })}
                </p>
            </div>

            {hasWinner && (
                <div className="text-center">
                    <p className="text-sm font-semibold text-yellow-600 dark:text-yellow-400">
                        {t("game.playerWon", { username: winnerUsername })}
                    </p>
                </div>
            )}

            {roundResult.eliminatedPlayerId && (
                <div className="text-center">
                    <p className="text-sm font-semibold text-red-600 dark:text-red-400">
                        {t("game.playerEliminated", {
                            username: gameState.players[roundResult.eliminatedPlayerId]?.username ?? "?"
                        })}
                    </p>
                </div>
            )}

            <div className="flex flex-col gap-2">
                <p className="text-xs font-semibold text-muted-foreground uppercase tracking-wide text-center">
                    {t("game.allCards")}
                </p>
                <div className="flex flex-wrap gap-2 justify-center">
                    {Object.entries(roundResult.allPlayerCards).flatMap(([pid, cards]) =>
                        cards.map((card, i) => (
                            <div key={`${pid}-${i}`} className="flex flex-col items-center gap-1">
                                <CardSprite suit={card.suit} rank={card.rank} displayWidth={60} />
                                <span className="text-xs text-muted-foreground">
                                    {gameState.players[pid]?.username}
                                </span>
                            </div>
                        ))
                    )}
                </div>
            </div>
        </div>
    );
};
