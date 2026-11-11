import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

import { useRequest } from "@/features/http/hooks/useRequest";
import { showError } from "@/features/shared/utils/toast";

import type { GameStatus } from "../models/GameStatus";

type UseGameStatusParams = {
    gameCode?: string;
};

export function useGameStatus({ gameCode }: UseGameStatusParams) {
    const navigate = useNavigate();
    const { t } = useTranslation();
    const { send } = useRequest();
    const requestedGameCode = useRef<string | undefined>(undefined);
    const [isLoading, setIsLoading] = useState(!!gameCode);

    useEffect(() => {
        if (!gameCode || requestedGameCode.current === gameCode) {
            return;
        }

        requestedGameCode.current = gameCode;
        setIsLoading(true);

        (async () => {
            try {
                const response = await send<void, GameStatus>({
                    method: "GET",
                    url: `/games/${gameCode}/status`,
                });

                if (!response.data.exists) {
                    showError(t("game.notFound"));
                    navigate("/");
                    return;
                }

                if (response.data.hasStarted) {
                    showError(t("game.alreadyStarted"));
                    navigate("/");
                    return;
                }

                setIsLoading(false);
            }
            catch {
                navigate("/");
            }
        })();
    }, [gameCode, navigate, send, t]);

    return { isLoading };
}
