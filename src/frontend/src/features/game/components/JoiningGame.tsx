import { type Dispatch, type SetStateAction, useCallback } from "react";
import { useTranslation } from "react-i18next";

import { Loader2 } from "lucide-react";

import { type HubMethods } from "@/features/http/hooks/useHub";
import { useAppDispatch } from "@/features/store/hooks";

import { GamePageState } from "../consts/GamePageState";
import { type JoinGameResponse, useJoinGame } from "../hooks/useJoinGame";
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

    const { t } = useTranslation();
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
        <div className="flex flex-col items-center justify-center gap-3 py-12">
            <Loader2 className="h-8 w-8 animate-spin" />
            <p>{t("game.joining")}</p>
        </div>
    )
}