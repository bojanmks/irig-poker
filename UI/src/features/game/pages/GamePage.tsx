import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import type { GameWonNotification } from "@/features/game/models/GameWonNotification";
import type { RoundResolvedNotification } from "@/features/game/models/RoundResolvedNotification";
import { resetGameState, setRoundResultData, setWinnerData } from "@/features/game/store/gameStateSlice";
import { useHub } from "@/features/http/hooks/useHub";
import SeoHead from "@/features/seo/components/SeoHead";
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";

import ActualGame from "../components/ActualGame";
import { ConnectingToServer } from "../components/ConnectingToServer";
import { EnterNameForm } from "../components/EnterNameForm";
import { GameLobby } from "../components/GameLobby";
import { JoiningGame } from "../components/JoiningGame";
import { RoundResultDisplay } from "../components/RoundResultDisplay";
import { GamePageState } from "../consts/GamePageState";
import { useDiconnectOnPageLeave } from "../hooks/useDisconnectOnPageLeave";
import { useGameHubDisconnectHandler } from "../hooks/useGameHubDisconnectHandler";
import { useGamePageEventListeners } from "../hooks/useGamePageEventListeners";
import { useGamePageWrapperClass } from "../hooks/useGamePageWrapperClass";
import { useUsernameFromRoute } from "../hooks/useUsernameFromRoute";

const GamePage = () => {
  const { onDisconnected } = useGameHubDisconnectHandler();

  const hub = useHub({ onDisconnected });
  const dispatch = useAppDispatch();
  const gameState = useAppSelector((state) => state.gameState.gameState);

  const { gameCode } = useParams();
  const { username, setUsername } = useUsernameFromRoute();
  const [pageState, setPageState] = useState<GamePageState>(GamePageState.Joining);

  const onGameWon = useCallback((data: GameWonNotification) => {
    dispatch(setWinnerData(data));
  }, [dispatch]);

  const onRoundResolved = useCallback((data: RoundResolvedNotification) => {
    dispatch(setRoundResultData(data));
  }, [dispatch]);

  useGamePageEventListeners(hub, { onGameWon, onRoundResolved });

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

  return (
    <>
      <SeoHead titleKey="appTitle" descriptionKey="metaDescriptionGame" />
      {gameState?.hasStarted ? (
        <ActualGame hub={hub} />
      ) : (
        <GameLobby />
      )}
      <RoundResultDisplay />
    </>
  );
};

export default GamePage;