import { type Dispatch, type SetStateAction, useEffect } from "react";
import { useTranslation } from "react-i18next";

import { Loader2 } from "lucide-react";

import { GamePageState } from "../consts/GamePageState";

type CheckingGameParams = {
    setPageState: Dispatch<SetStateAction<GamePageState>>;
};

export function CheckingGame(params: CheckingGameParams) {
    const { setPageState } = params;
    const { t } = useTranslation();

    useEffect(() => {
        setPageState(GamePageState.Checking);

        return () => {
            setPageState(GamePageState.Joining);
        };
    }, [setPageState]);

    return (
        <div className="flex flex-col items-center justify-center gap-3 py-12">
            <Loader2 className="h-8 w-8 animate-spin" />
            <p>{t("game.checkingGame")}</p>
        </div>
    );
}
