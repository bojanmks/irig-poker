import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import { resetGameState } from "@/features/game/store/gameStateSlice";
import { useHub } from "@/features/http/hooks/useHub";
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";

import ActualGame from "../components/ActualGame";
import { ConnectingToServer } from "../components/ConnectingToServer";
import { EnterNameForm } from "../components/EnterNameForm";
import { GameLobby } from "../components/GameLobby";
import { JoiningGame } from "../components/JoiningGame";
import { GamePageState } from "../consts/GamePageState";
import { useDiconnectOnPageLeave } from "../hooks/useDisconnectOnPageLeave";
import { useGamePageEventListeners } from "../hooks/useGamePageEventListeners";
import { useGamePageWrapperClass } from "../hooks/useGamePageWrapperClass";
import { useUsernameFromRoute } from "../hooks/useUsernameFromRoute";

const GamePage = () => {
  const hub = useHub();
  const dispatch = useAppDispatch();
  const gameState = useAppSelector((state) => state.gameState.gameState);

  const { gameCode } = useParams();
  const { username, setUsername } = useUsernameFromRoute();
  const [pageState, setPageState] = useState<GamePageState>(GamePageState.Joining);

  useGamePageEventListeners(hub);

  useGamePageWrapperClass(pageState);

  useEffect(() => {
    return () => {
      dispatch(resetGameState());
    };
  }, [dispatch]);

  useDiconnectOnPageLeave(hub.disconnect);

  if (!hub.connected) {
    return <ConnectingToServer setPageState={setPageState} />
  }

  if (!username) {
    return <EnterNameForm setUsername={setUsername} setPageState={setPageState} />;
  }

  if (pageState === GamePageState.Joining) {
    return <JoiningGame hub={hub} gameCode={gameCode!} username={username} setPageState={setPageState} />;
  }

  return gameState?.hasStarted ? <ActualGame /> : <GameLobby />;
};

export default GamePage;