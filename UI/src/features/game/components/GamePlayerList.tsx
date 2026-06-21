import {
  Avatar,
  AvatarFallback,
} from "@/features/shared/components/shadcn/Avatar";
import {
  Card,
  CardContent,
} from "@/features/shared/components/shadcn/Card";
import { useAppSelector } from "@/features/store/hooks";

export const GamePlayerList = () => {
    const gameState = useAppSelector((state) => state.gameState.gameState);

    const players = gameState?.players
        ? Object.values(gameState.players)
        : [];

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-3">
            {players.map((player) => (
                <Card
                    key={player.playerId}
                    className="flex flex-col items-center gap-4 p-5"
                >
                    <Avatar className="w-14 h-14 ring-2 ring-border">
                        <AvatarFallback className="text-lg">
                            {player.username[0]?.toUpperCase()}
                        </AvatarFallback>
                    </Avatar>
                    <CardContent className="flex flex-col items-center gap-1 p-0">
                        <span className="font-semibold text-base">{player.username}</span>
                    </CardContent>
                </Card>
            ))}
        </div>
    )
};