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
  const { connected: hubConnected, on: hubOn, off: hubOff, invoke: hubInvoke } = hub;

  const hasJoinedGame = useRef(false);

  useEffect(() => {
    if (hubConnected) {
      hubOn("PlayerJoined", onPlayerJoined);
      hubOn("PlayerLeft", onPlayerLeft);
      hubOn("GameStarted", onGameStarted);
    }

    return () => {
      hubOff("PlayerJoined");
      hubOff("PlayerLeft");
      hubOff("GameStarted");
    }
  }, [hubConnected, hubOn, hubOff, onPlayerJoined, onPlayerLeft, onGameStarted]);

  useEffect(() => {
    if (hasJoinedGame.current || !hubConnected || !username) return;

    (async () => {
      const response = await hubInvoke<PublicGameState>("JoinGame", { gameCode, username });

      if (!response.isSuccess) {
        navigate('/');
        return;
      }

      hasJoinedGame.current = true;
      onJoined?.(response.data!);
    })();
  }, [hubConnected, hubInvoke, gameCode, username, onJoined, navigate]);
}