import { type Dispatch, type SetStateAction, useEffect } from "react";
import { useTranslation } from "react-i18next";

import { Loader2 } from "lucide-react";

import { GamePageState } from "../consts/GamePageState";

type ConnectingToServerParams = {
    setPageState: Dispatch<SetStateAction<GamePageState>>;
}

export function ConnectingToServer(params: ConnectingToServerParams) {
    const { setPageState } = params;
    const { t } = useTranslation();

    useEffect(() => {
        setPageState(GamePageState.Connecting);

        return () => {
            setPageState(GamePageState.Joining);
        }
    }, [setPageState]);

    return (
        <div className="flex flex-col items-center justify-center gap-3 py-12">
            <Loader2 className="h-8 w-8 animate-spin" />
            <p>{t("game.connecting")}</p>
        </div>
    )
}