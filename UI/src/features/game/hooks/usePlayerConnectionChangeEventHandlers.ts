import { useCallback } from "react";
import { useTranslation } from "react-i18next";

import type { HubNotification } from "@/features/http/models/HubNotification";
import { showWarning } from "@/features/shared/utils/toast";
import { useAppSelector } from "@/features/store/hooks";

export function usePlayerConnectionChangeEventHandlers() {
    const { t } = useTranslation();
    const gameState = useAppSelector((state) => state.gameState.gameState);

    const onPlayerLeft = useCallback((notification: HubNotification<string>) => {
        const playerId = notification.data;
        const username = gameState?.players[playerId]?.username;
        if (username) {
            showWarning(t("game.playerLeftWarning", { username }));
        }
    }, [gameState, t]);

    return {
        onPlayerLeft,
    }
}