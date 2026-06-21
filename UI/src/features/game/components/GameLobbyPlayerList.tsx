import { useMemo } from "react";
import { useTranslation } from "react-i18next";

import { CrownIcon, UsersIcon } from "lucide-react";

import {
  Avatar,
  AvatarFallback,
} from "@/features/shared/components/shadcn/Avatar";
import {
  Card,
  CardContent,
} from "@/features/shared/components/shadcn/Card";
import { useAppSelector } from "@/features/store/hooks";

export const GameLobbyPlayerList = () => {
    const gameState = useAppSelector((state) => state.gameState.gameState);
    const playerId = useAppSelector((state) => state.gameState.playerId);
    const { t } = useTranslation();

    const players = useMemo(() => {
        return gameState ? Object.values(gameState.players) : [];
    }, [gameState]);

    return (
        <>
            <div className="flex items-center mb-4 gap-2">
                <UsersIcon className="w-5 h-5 text-muted-foreground" />
                <span className="font-semibold">{t("game.players")}</span>
            </div>
            <div className="space-y-2">
                {players.map((player) => {
                    const isAdmin = player.isAdmin;
                    const isSelf = player.playerId === playerId;
                    return (
                        <Card
                            key={player.playerId}
                            className={`flex flex-row items-center gap-3 p-3 ${isSelf ? "ring-2 ring-primary" : ""}`}
                        >
                            <Avatar>
                                <AvatarFallback>
                                    {player.username[0]?.toUpperCase()}
                                </AvatarFallback>
                            </Avatar>
                            <CardContent className="flex items-center gap-2 p-0">
                                <span className="font-medium">{player.username}</span>
                                {isAdmin && (
                                    <CrownIcon className="w-4 h-4 text-yellow-500" />
                                )}
                            </CardContent>
                        </Card>
                    );
                })}
            </div>
        </>
    )
};