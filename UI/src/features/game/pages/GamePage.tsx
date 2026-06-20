import { useParams } from "react-router-dom";
import { useCallback, useState } from "react";
import { GameLobby } from "../components/GameLobby";
import { GamePageState } from "../consts/GamePageState";
import { useUsernameFromRoute } from "../hooks/useUsernameFromRoute";
import { useJoinGame } from "../hooks/useJoinGame";
import { EnterNameForm } from "../components/EnterNameForm";
import ActualGame from "../components/ActualGame";
import { useGamePageWrapperClass } from "../hooks/useGamePageWrapperClass";
import { useDiconnectOnPageLeave } from "../hooks/useDisconnectOnPageLeave";
import { useHub } from "@/features/http/hooks/useHub";
import { GamePageLoading } from "../components/GamePageLoading";
import { useGamePageInitialization } from "../hooks/useGamePageInitialization";
import type { PublicGameState } from "../models/PublicGameState";
import { GameStateContext, useGameState } from "../contexts/GameStateContext";

const GamePageContent = () => {
  const hub = useHub();
  const { gameCode } = useParams();
  const { username, setUsername, initialized } = useUsernameFromRoute();
  const [pageState, setPageState] = useState<GamePageState>(GamePageState.None);
  const { gameState, setGameState } = useGameState();

  useGamePageWrapperClass(pageState);

  useJoinGame({
    hub,
    gameCode: gameCode!,
    username: username,
    onJoined: useCallback((gameState: PublicGameState) => {
      if (([GamePageState.None, GamePageState.EnterNameToJoin] as GamePageState[]).includes(pageState)) {
        setPageState(GamePageState.Ready)
      }
      
      setGameState(gameState);
    }, [pageState, setPageState, setGameState])
  });

  useDiconnectOnPageLeave(hub.disconnect);

  useGamePageInitialization({
    username,
    usernameInitialized: initialized,
    pageState,
    setPageState
  });

  if (pageState === GamePageState.None) {
    return <GamePageLoading />
  }

  if (pageState === GamePageState.EnterNameToJoin) {
    return <EnterNameForm onSubmit={(name) => {
      setUsername(name);
    }} />;
  }

  return (
    <>
      { gameState?.hasStarted ? <ActualGame /> : <GameLobby /> }
    </>
  );
};

const GamePage = () => {
  const [gameState, setGameState] = useState<PublicGameState | null>(null);

  return (
    <GameStateContext.Provider value={{ gameState, setGameState }}>
      <GamePageContent />
    </GameStateContext.Provider>
  );
};

export default GamePage;