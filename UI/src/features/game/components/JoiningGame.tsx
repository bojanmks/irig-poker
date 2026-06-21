import { type Dispatch, type SetStateAction,useCallback } from "react";

import { type HubMethods } from "@/features/http/hooks/useHub";
import { useAppDispatch } from "@/features/store/hooks";

import { GamePageState } from "../consts/GamePageState";
import { type JoinGameResponse,useJoinGame } from "../hooks/useJoinGame";
import { setGameState } from "../store/gameStateSlice";

type JoiningGameParams = {
    hub: HubMethods;
    gameCode: string;
    username: string;
    setPageState: Dispatch<SetStateAction<GamePageState>>
};

export function JoiningGame(params: JoiningGameParams) {
    const {
        hub,
        gameCode,
        username,
        setPageState
    } = params;
    
    const dispatch = useAppDispatch();

    const onJoined = useCallback(({ gameState, playerId }: JoinGameResponse) => {
        dispatch(setGameState({ gameState, playerId }));
        setPageState(GamePageState.Ready);
    }, [dispatch, setPageState]);

    useJoinGame({
        hub,
        gameCode,
        username,
        onJoined
    });

    return (
        <>Joining</>
    )
}