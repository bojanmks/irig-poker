import { useEffect, useRef, type SetStateAction } from "react";
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
    const initialized = useRef(false);

    useEffect(() => {
        if (initialized.current || !usernameInitialized) {
            return;
        }

        if (!username && pageState === GamePageState.None) {
            setPageState(GamePageState.EnterNameToJoin);
        }

        initialized.current = true;
    }, [usernameInitialized, username, pageState, setPageState]);
}