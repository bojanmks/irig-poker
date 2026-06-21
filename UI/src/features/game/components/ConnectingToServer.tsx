import { type Dispatch, type SetStateAction,useEffect } from "react";

import { GamePageState } from "../consts/GamePageState";

type ConnectingToServerParams = {
    setPageState: Dispatch<SetStateAction<GamePageState>>;
}

export function ConnectingToServer(params: ConnectingToServerParams) {
    const {setPageState } = params;

    useEffect(() => {
        setPageState(GamePageState.Connecting);

        return () => {
            setPageState(GamePageState.Joining);
        }
    }, [setPageState]);

    return (
        <>Connecting</>
    )
}