import { useEffect, useState, type SetStateAction } from "react";
import { GamePageState } from "../consts/GamePageState";

export function useGamePageInitialization({
    username,
    usernameInitialized,
    pageState,
    setPageState
}: {
    username: string | null;
    usernameInitialized: boolean;
    pageState: GamePageState,
    setPageState: (value: SetStateAction<GamePageState>) => void;
}) {
    const [gamePageInitialized, setGamePageInitialized] = useState(false);

    useEffect(() => {
        if (gamePageInitialized || !usernameInitialized) {
            return;
        }

        if (!username && pageState === GamePageState.None) {
            setPageState(GamePageState.EnterNameToJoin);
        }

        setGamePageInitialized(true);
    }, [gamePageInitialized, setGamePageInitialized, usernameInitialized, username]);
}