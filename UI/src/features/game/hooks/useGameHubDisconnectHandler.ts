import { useCallback } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

import { showError } from "@/features/shared/utils/toast";

export function useGameHubDisconnectHandler() {
    const navigate = useNavigate();
    const { t } = useTranslation();

    const onDisconnected = useCallback(() => {
        navigate("/");
        showError(t("game.disconnected"));
    }, [navigate]);

    return { onDisconnected }
}