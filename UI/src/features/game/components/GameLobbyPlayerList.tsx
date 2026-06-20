import { useTranslation } from "react-i18next";
import { Avatar, AvatarFallback } from "@radix-ui/react-avatar";

import { UsersIcon } from "lucide-react";

import { useAppSelector } from "@/features/store/hooks";

export const GameLobbyPlayerList = () => {
    const gameState = useAppSelector((state) => state.gameState.gameState);
    const { t } = useTranslation();

    return (
        <>
            <div className="flex items-center mb-4 gap-2">
                <UsersIcon className="w-5 h-5 text-muted-foreground" />
                <span className="font-semibold">{t("game.players")}</span>
            </div>
            <div className="space-y-2">
                {gameState?.players &&
                    Object.keys(gameState.players).map((playerId) => {
                        const player = gameState.players[playerId];
                        return (
                            <div
                                key={playerId}
                                className="flex items-center gap-3 p-3 border rounded-lg hover:bg-muted transition-colors"
                            >
                                <Avatar>
                                    <AvatarFallback>
                                        {player.username[0]?.toUpperCase()}
                                    </AvatarFallback>
                                </Avatar>
                                <span className="font-medium">{player.username}</span>
                            </div>
                        );
                    })}
            </div>
        </>
    )
};