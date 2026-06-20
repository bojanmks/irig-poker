import { useEffect, useRef } from "react";
import { type HubMethods } from "@/features/http/hooks/useHub";
import { useNavigate } from "react-router-dom";
import type { PublicGameState } from "../models/PublicGameState";
import { usePlayerConnectionChangeListeners } from "./usePlayerConnectionChangeListeners";
import { useGameStartListeners } from "./useGameStartListeners";

type UseJoinGameParams = {
  hub: HubMethods,
  gameCode: string,
  username: string | null,
  onJoined?: (gameState: PublicGameState) => void
}

export function useJoinGame({
  hub,
  gameCode,
  username,
  onJoined
}: UseJoinGameParams) {
  const navigate = useNavigate();
  const { onPlayerJoined, onPlayerLeft } = usePlayerConnectionChangeListeners();
  const { onGameStarted } = useGameStartListeners();

  const hasJoinedGame = useRef(false);

  useEffect(() => {
    if (hub.connected) {
      hub.on("PlayerJoined", onPlayerJoined);
      hub.on("PlayerLeft", onPlayerLeft);
      hub.on("GameStarted", onGameStarted);
    }

    return () => {
      hub.off("PlayerJoined");
      hub.off("PlayerLeft");
      hub.off("GameStarted");
    }
  }, [hub.connected, hub.on, hub.off, onPlayerJoined, onPlayerLeft, onGameStarted]);

  useEffect(() => {
    if (hasJoinedGame.current || !hub.connected || !username) return;

    (async () => {
      const response = await hub.invoke<PublicGameState>("JoinGame", { gameCode, username });

      if (!response.isSuccess) {
        navigate('/');
        return;
      }

      hasJoinedGame.current = true;
      onJoined?.(response.data!);
    })();
  }, [hasJoinedGame.current, hub.connected, gameCode, username, onJoined, navigate]);
}