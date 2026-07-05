import { useCallback, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

import type { HubMethods } from "@/features/http/hooks/useHub";
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
import { cn } from "@/lib/utils";

import { HandType } from "../models/HandType";
import { Rank } from "../models/Rank";
import { describeRanks } from "../utils/describeRanks";

import { ClaimedHandCards } from "./ClaimedHandCards";
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

type ClaimHandPanelProps = {
    hub: HubMethods;
};

type ClaimHandDialogProps = {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    onClaim: (handType: HandType, ranks: Rank[], suit?: number) => void;
    currentClaimedHand: HandType | null;
    currentRanks: Rank[] | null;
    actionLoading: boolean;
    labelKey: string;
};

const ClaimHandDialog = ({ open, onOpenChange, onClaim, currentClaimedHand, currentRanks, actionLoading, labelKey }: ClaimHandDialogProps) => {
    const { t } = useTranslation();

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogTrigger asChild>
                <Button variant="outline" size="sm" loading={actionLoading}>
                    {t(labelKey)}
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[calc(100%-2rem)] lg:max-w-4xl">
                <DialogHeader>
                    <DialogTitle>{t(labelKey)}</DialogTitle>
                </DialogHeader>
                <HandSelector
                    onSelect={onClaim}
                    currentClaimedHand={currentClaimedHand}
                    currentRanks={currentRanks}
                    disabled={actionLoading}
                />
            </DialogContent>
        </Dialog>
    );
};

export const ClaimHandPanel = ({ hub }: ClaimHandPanelProps) => {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const gameState = useAppSelector((state) => state.gameState.gameState);
    const playerId = useAppSelector((state) => state.gameState.playerId);
    const winner = useAppSelector((state) => state.gameState.winnerData);
    const [dialogOpen, setDialogOpen] = useState(false);
    const [actionLoading, setActionLoading] = useState(false);
    const isLg = useMediaQuery("(min-width: 1024px)");
    const cardSpriteWidth = isLg ? 80 : 60;
    const panelHeightClass = isLg ? "h-80" : "h-60";

    const handleCallBluff = useCallback(async () => {
        setActionLoading(true);
        try {
            await hub.invoke("CallBluff", {});
        } finally {
            setActionLoading(false);
        }
    }, [hub]);

    const handleClaim = useCallback(async (handType: HandType, ranks: Rank[], suit?: number) => {
        setActionLoading(true);
        try {
            await hub.invoke("ClaimHand", { claimedHand: handType, ranks, suit });
            setDialogOpen(false);
        } finally {
            setActionLoading(false);
        }
    }, [hub]);

    if (!gameState) {
        return null;
    }

    if (winner) {
        const isSelf = winner.winnerPlayerId === playerId;
        return (
            <div className={cn(panelHeightClass, "p-4 rounded-lg border border-border bg-card/50 flex flex-col items-center justify-center gap-3")}>
                <p className="text-lg font-semibold text-center">
                    {isSelf ? t("game.youWon") : t("game.playerWon", { username: winner.winnerUsername })}
                </p>
                <Button onClick={() => navigate("/")}>
                    {t("game.createNewGame")}
                </Button>
            </div>
        );
    }

    const isMyTurn = gameState.currentTurnPlayerId === playerId;
    const currentClaim = gameState.currentClaimedHand;
    const currentClaimerId = gameState.claimingPlayerId;
    const currentClaimer = currentClaimerId ? gameState.players[currentClaimerId] : null;
    const currentRanks = gameState.ranks;
    const currentTurnPlayer = gameState.currentTurnPlayerId ? gameState.players[gameState.currentTurnPlayerId] : null;

    return (
        <div className={cn(panelHeightClass, "p-4 rounded-lg border border-border bg-card/50")}>
            {currentClaimer && currentClaim !== null && currentRanks !== null ? (
                <div className="flex flex-col items-center gap-3 h-full justify-center">
                    <div className="text-sm text-muted-foreground text-center truncate max-w-full">
                        <span className="font-semibold text-foreground">{currentClaimer.username}</span>
                        {" "}{t("game.claimed")}{" "}
                        <span className="font-semibold text-foreground">{t(handTypeLabels[currentClaim])}</span>
                        {" "}({describeRanks(currentClaim, currentRanks, gameState.claimedSuit)})
                    </div>
                    <ClaimedHandCards
                        handType={currentClaim}
                        ranks={currentRanks}
                        suit={gameState.claimedSuit}
                        displayWidth={cardSpriteWidth}
                    />
                    {isMyTurn ? (
                        <div className="flex flex-col items-center gap-2">
                            <Button
                                onClick={handleCallBluff}
                                variant="destructive"
                                size="sm"
                                loading={actionLoading}
                            >
                                {t("game.callBluff")}
                            </Button>
                            <ClaimHandDialog
                                open={dialogOpen}
                                onOpenChange={setDialogOpen}
                                onClaim={handleClaim}
                                currentClaimedHand={currentClaim}
                                currentRanks={currentRanks}
                                actionLoading={actionLoading}
                                labelKey="game.raise"
                            />
                        </div>
                    ) : (
                        <p className="text-xs text-muted-foreground text-center">
                            {t("game.waitingForPlayer", { username: currentTurnPlayer?.username ?? "" })}
                        </p>
                    )}
                </div>
            ) : isMyTurn ? (
                <div className="flex flex-col items-center justify-center gap-3 h-full">
                    <p className="text-sm text-muted-foreground text-center">
                        {t("game.startingRound")}
                    </p>
                    <ClaimHandDialog
                        open={dialogOpen}
                        onOpenChange={setDialogOpen}
                        onClaim={handleClaim}
                        currentClaimedHand={currentClaim}
                        currentRanks={currentRanks}
                        actionLoading={actionLoading}
                        labelKey="game.open"
                    />
                </div>
            ) : (
                <div className="flex items-center justify-center h-full">
                    <p className="text-sm text-muted-foreground text-center">
                        {t("game.waitingForPlayer", { username: currentTurnPlayer?.username ?? "" })}
                    </p>
                </div>
            )}
        </div>
    );
};
