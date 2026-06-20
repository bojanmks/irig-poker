import { useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";

import { type HubMethods } from "@/features/http/hooks/useHub";

import type { PublicGameState } from "../models/PublicGameState";

import { useGameStartListeners } from "./useGameStartListeners";
import { usePlayerConnectionChangeListeners } from "./usePlayerConnectionChangeListeners";

type JoinGameResponse = {
  playerId: string;
  gameState: PublicGameState;
};

type UseJoinGameParams = {
  hub: HubMethods,
  gameCode: string,
  username: string | null,
  onJoined?: (response: JoinGameResponse) => void
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
      const response = await hubInvoke<JoinGameResponse>("JoinGame", { gameCode, username });

      if (!response.isSuccess) {
        navigate('/');
        return;
      }

      hasJoinedGame.current = true;
      onJoined?.(response.data!);
    })();
  }, [hubConnected, hubInvoke, gameCode, username, onJoined, navigate]);
}