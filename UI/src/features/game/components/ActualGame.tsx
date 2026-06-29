import type { HubMethods } from "@/features/http/hooks/useHub";

import type { RoundResolvedNotification } from "../models/RoundResolvedNotification";

import { ClaimHandPanel } from "./ClaimHandPanel";
import { GamePlayerList } from "./GamePlayerList";
import { PlayerHand } from "./PlayerHand";
import { RoundResultDisplay } from "./RoundResultDisplay";

type ActualGameProps = {
    hub: HubMethods;
    roundResultData: RoundResolvedNotification | null;
    onDismissRoundResult: () => void;
};

const ActualGame = ({ hub, roundResultData, onDismissRoundResult }: ActualGameProps) => {
    return (
        <div className="flex flex-col gap-4">
            <ClaimHandPanel hub={hub} />
            <RoundResultDisplay roundResult={roundResultData} onDismiss={onDismissRoundResult} />
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