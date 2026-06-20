import { useParams } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
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
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";
import { setGameState, resetGameState } from "@/features/game/store/gameStateSlice";

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
    onJoined: useCallback((gameState: PublicGameState) => {
      if (([GamePageState.None, GamePageState.EnterNameToJoin] as GamePageState[]).includes(pageState)) {
        setPageState(GamePageState.Ready)
      }
      
      dispatch(setGameState(gameState));
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