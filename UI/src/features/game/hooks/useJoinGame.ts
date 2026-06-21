import { useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";

import { type HubMethods } from "@/features/http/hooks/useHub";

import type { PublicGameState } from "../models/PublicGameState";

export type JoinGameResponse = {
  playerId: string;
  gameState: PublicGameState;
};

type UseJoinGameParams = {
  hub: HubMethods,
  gameCode: string,
  username: string,
  onJoined?: (response: JoinGameResponse) => void
}

export function useJoinGame({
  hub,
  gameCode,
  username,
  onJoined
}: UseJoinGameParams) {
  const navigate = useNavigate();
  const { connected: hubConnected, invoke: hubInvoke } = hub;
  const invoked = useRef(false);

  useEffect(() => {
    if (!hubConnected || invoked.current) return;
    invoked.current = true;

    (async () => {
      const response = await hubInvoke<JoinGameResponse>("JoinGame", { gameCode, username });

      if (!response.isSuccess) {
        navigate('/');
        return;
      }

      onJoined?.(response.data!);
    })();
  }, [hubConnected, hubInvoke, gameCode, username, onJoined, navigate]);
}