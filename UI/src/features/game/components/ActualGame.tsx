import { useTranslation } from "react-i18next";

import type { HubMethods } from "@/features/http/hooks/useHub";
import { useAppSelector } from "@/features/store/hooks";

import { ClaimHandPanel } from "./ClaimHandPanel";
import { GamePlayerList } from "./GamePlayerList";
import { PlayerHand } from "./PlayerHand";

type ActualGameProps = {
    hub: HubMethods;
};

const ActualGame = ({ hub }: ActualGameProps) => {
    const { t } = useTranslation();
    const gameState = useAppSelector((state) => state.gameState.gameState);

    return (
        <div className="flex flex-col gap-4">
            <span className="text-xs text-muted-foreground">
                {t("game.eliminatedAt", { max: gameState?.maxCardCount })}
            </span>
            <ClaimHandPanel hub={hub} />
            <div className="flex flex-col lg:flex-row gap-4">
                <div className="lg:w-1/3 xl:w-1/4 shrink-0 order-2 lg:order-1">
                    <div className="p-4">
                        <PlayerHand />
                    </div>
                </div>
                <div className="lg:w-2/3 xl:w-3/4 flex-1 order-1 lg:order-2">
                    <div className="p-1 overflow-y-auto max-h-[50dvh] lg:max-h-[80dvh]">
                        <GamePlayerList />
                    </div>
                </div>
            </div>
        </div>
    )
}

export default ActualGame;