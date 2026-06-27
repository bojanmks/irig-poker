import { GamePlayerList } from "./GamePlayerList";
import { PlayerHand } from "./PlayerHand";

const ActualGame = () => {
    return (
        <div className="flex flex-col lg:flex-row h-dvh max-h-dvh">
            <div className="lg:w-1/2 xl:w-1/4 shrink-0 overflow-y-auto order-2 lg:order-1">
                <div className="p-4">
                    <PlayerHand />
                </div>
            </div>
            <div className="lg:w-1/2 xl:w-3/4 flex-1 overflow-y-auto order-1 lg:order-2">
                <div className="p-4">
                    <GamePlayerList />
                </div>
            </div>
        </div>
    )
}

export default ActualGame;