import { useTranslation } from "react-i18next";

import { ScrollText } from "lucide-react";

import { Button } from "@/features/shared/components/shadcn/Button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/features/shared/components/shadcn/Dialog";
import { useAppSelector } from "@/features/store/hooks";
import { useMediaQuery } from "@/lib/useMediaQuery";

import { HandType } from "../models/HandType";
import { describeRanks } from "../utils/describeRanks";

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

export const RoundHistoryDialog = () => {
  const { t } = useTranslation();
  const isLg = useMediaQuery("(min-width: 1024px)");
  const cardWidth = isLg ? 80 : 60;
  const gameState = useAppSelector((state) => state.gameState.gameState);
  const roundHistory = gameState?.roundHistory ?? [];

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline" size="icon" aria-label={t("game.roundHistory")}>
          <ScrollText className="h-4 w-4" />
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-md max-h-[90dvh] flex flex-col">
        <DialogHeader>
          <DialogTitle>{t("game.roundHistory")}</DialogTitle>
        </DialogHeader>
        <div className="flex-1 overflow-y-auto min-h-0">
          {roundHistory.length === 0 ? (
            <p className="text-sm text-muted-foreground text-center py-8">
              {t("game.roundHistoryEmpty")}
            </p>
          ) : (
            <div className="flex flex-col gap-4">
              {roundHistory.map((entry, index) => (
                <div key={index} className="flex flex-col items-center gap-2 pb-4 border-b border-border last:border-b-0">
                  <p className="text-sm text-muted-foreground">
                    <span className="font-semibold text-foreground">
                      {gameState?.players[entry.claimingPlayerId]?.username}
                    </span>
                    {" "}{t("game.claimed")}{" "}
                    <span className="font-semibold text-foreground">
                      {t(handTypeLabels[entry.claimedHand])}
                    </span>
                    {" "}({describeRanks(entry.claimedHand, entry.ranks, entry.suit)})
                  </p>
                  <ClaimedHandCards
                    handType={entry.claimedHand}
                    ranks={entry.ranks}
                    suit={entry.suit}
                    displayWidth={cardWidth}
                  />
                </div>
              ))}
            </div>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
};
