import { CrownIcon } from "lucide-react";

import {
  Avatar,
  AvatarFallback,
} from "@/features/shared/components/shadcn/Avatar";
import {
  Card,
  CardContent,
} from "@/features/shared/components/shadcn/Card";

type GameLobbyPlayerCardProps = {
  player: {
    playerId: string;
    username: string;
    isAdmin: boolean;
  };
  isSelf: boolean;
};

export const GameLobbyPlayerCard = ({ player, isSelf }: GameLobbyPlayerCardProps) => {
  return (
    <Card
      className={`flex flex-row items-center gap-3 p-3 ${isSelf ? "ring-2 ring-primary" : ""}`}
    >
      <Avatar>
        <AvatarFallback>
          {player.username[0]?.toUpperCase()}
        </AvatarFallback>
      </Avatar>
      <CardContent className="flex items-center gap-2 p-0">
        <span className="font-medium">{player.username}</span>
        {player.isAdmin && (
          <CrownIcon className="w-4 h-4 text-yellow-500" />
        )}
      </CardContent>
    </Card>
  );
};
