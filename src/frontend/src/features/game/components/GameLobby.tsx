import { CopyGameLinkButton } from "./CopyGameLinkButton";
import { GameLobbyPlayerList } from "./GameLobbyPlayerList";
import { StartGameButton } from "./StartGameButton";

export const GameLobby = () => {
  return (
    <div className="space-y-6">
      <GameLobbyPlayerList />
      
      <div className="space-y-2">
        <StartGameButton />
        <CopyGameLinkButton />
      </div>
    </div>
  );
};