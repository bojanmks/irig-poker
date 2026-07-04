import { useMemo } from "react";
import { useTranslation } from "react-i18next";

import { Button } from "@/features/shared/components/shadcn/Button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/features/shared/components/shadcn/Dialog";
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";

import { HandType } from "../models/HandType";
import { Rank } from "../models/Rank";
import { Suit } from "../models/Suit";
import { clearRoundResultData } from "../store/gameStateSlice";

import { CardSprite } from "./CardSprite";
import { ClaimedHandCards } from "./ClaimedHandCards";

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

function rankLabel(rank: Rank): string {
  switch (rank) {
    case Rank.Two: return "2";
    case Rank.Three: return "3";
    case Rank.Four: return "4";
    case Rank.Five: return "5";
    case Rank.Six: return "6";
    case Rank.Seven: return "7";
    case Rank.Eight: return "8";
    case Rank.Nine: return "9";
    case Rank.Ten: return "10";
    case Rank.Jack: return "J";
    case Rank.Queen: return "Q";
    case Rank.King: return "K";
    case Rank.Ace: return "A";
    default:
      rank satisfies never;   
      return "";
  }
}

function describeRanks(handType: HandType, ranks: Rank[], suit?: number | null): string {
  if (ranks.length === 0) return "";
  if (handType === HandType.TwoPair) {
    return `${rankLabel(ranks[0])} & ${rankLabel(ranks[1])}`;
  }
  if (handType === HandType.FullHouse) {
    return `3x${rankLabel(ranks[0])} & 2x${rankLabel(ranks[1])}`;
  }
  const label = rankLabel(ranks[0]);
  if (handType === HandType.StraightFlush && suit !== null && suit !== undefined) {
    return `${label}${suitSymbols[suit] ?? ""}`;
  }
  return label;
}

export const RoundResultDisplay = () => {
  const { t } = useTranslation();
  const dispatch = useAppDispatch();
  const roundResult = useAppSelector((state) => state.gameState.roundResultData);
  const gameState = useAppSelector((state) => state.gameState.gameState);

  const hasWinner = roundResult?.winnerPlayerId != null;

  const winnerUsername = useMemo(() => {
    return roundResult?.winnerUsername ?? "";
  }, [roundResult]);

  const handleDismiss = useMemo(() => {
    return () => dispatch(clearRoundResultData());
  }, [dispatch]);

  if (!roundResult || !gameState) {
    return null;
  }

  const loserUsername = gameState.players[roundResult.losingPlayerId]?.username ?? "?";

  return (
    <Dialog open onOpenChange={(open) => { if (!open) handleDismiss(); }}>
      <DialogContent className="sm:max-w-lg max-h-[90dvh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>{t("game.roundResult")}</DialogTitle>
        </DialogHeader>

        <div className="flex flex-col gap-4">
          <div className="flex flex-col items-center gap-2">
            <p className="text-sm text-muted-foreground">
              <span className="font-semibold text-foreground">
                {gameState.players[roundResult.claimingPlayerId]?.username}
              </span>
              {" "}{t("game.claimed")}{" "}
              <span className="font-semibold text-foreground">
                {t(handTypeLabels[roundResult.claimedHand])}
              </span>
              {" "}({describeRanks(roundResult.claimedHand, roundResult.ranks, roundResult.suit)})
            </p>
            <ClaimedHandCards
              handType={roundResult.claimedHand}
              ranks={roundResult.ranks}
              suit={roundResult.suit}
            />
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

        <div className="flex justify-center mt-2">
          <Button onClick={handleDismiss}>
            {t("game.close")}
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
};
