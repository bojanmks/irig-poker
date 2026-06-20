import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import { resetGameState,setGameState } from "@/features/game/store/gameStateSlice";
import { useHub } from "@/features/http/hooks/useHub";
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";

import ActualGame from "../components/ActualGame";
import { EnterNameForm } from "../components/EnterNameForm";
import { GameLobby } from "../components/GameLobby";
import { GamePageLoading } from "../components/GamePageLoading";
import { GamePageState } from "../consts/GamePageState";
import { useDiconnectOnPageLeave } from "../hooks/useDisconnectOnPageLeave";
import { useGamePageInitialization } from "../hooks/useGamePageInitialization";
import { useGamePageWrapperClass } from "../hooks/useGamePageWrapperClass";
import { useJoinGame } from "../hooks/useJoinGame";
import { useUsernameFromRoute } from "../hooks/useUsernameFromRoute";

const GamePage = () => {
  const hub = useHub();
  const { gameCode } = useParams();
  const { username, setUsername, initialized } = useUsernameFromRoute();
  const [pageState, setPageState] = useState<GamePageState>(GamePageState.None);
  const dispatch = useAppDispatch();
  const gameState = useAppSelector((state) => state.gameState.gameState);

  useGamePageWrapperClass(pageState);

  useEffect(() => {
    return () => {
      dispatch(resetGameState());
    };
  }, [dispatch]);

  useJoinGame({
    hub,
    gameCode: gameCode!,
    username: username,
    onJoined: useCallback(({ gameState, playerId: yourPlayerId }) => {
      if (([GamePageState.None, GamePageState.EnterNameToJoin] as GamePageState[]).includes(pageState)) {
        setPageState(GamePageState.Ready)
      }
      
      dispatch(setGameState({ gameState, playerId: yourPlayerId }));
    }, [pageState, setPageState, dispatch])
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

export default GamePage;