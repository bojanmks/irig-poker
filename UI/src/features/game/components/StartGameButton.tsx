import { useHub } from "@/features/http/hooks/useHub";
import { usePlayerInfo } from "../hooks/usePlayerInfo";
import { useTranslation } from "react-i18next";
import { Button } from "@/features/shared/components/shadcn/Button";
import { useCallback, useMemo, useState } from "react";
import { useAppSelector } from "@/features/store/hooks";

export function StartGameButton() {
    const { t } = useTranslation();
    const { isAdmin } = usePlayerInfo();
    const { invoke } = useHub();
    const gameState = useAppSelector((state) => state.gameState.gameState);
    const [showLoading, setShowLoading] = useState(false);

    const currentPlayerCount = useMemo(() => {
        return Object.keys(gameState?.players || {}).length;
    }, [gameState?.players]);
    
    const hasEnoughPlayers = useMemo(() => {
        return currentPlayerCount >= import.meta.env.VITE_MIN_PLAYERS_TO_START;
    }, [currentPlayerCount]);

    const startGame = useCallback(async () => {
        setShowLoading(true);

        try {
            await invoke<void>("StartGame");
        }
        finally {
            setShowLoading(false);
        }
    }, [invoke, setShowLoading]);

    if (!isAdmin) {
        return null;
    }

    return (
        <Button
            type="button"
            loading={showLoading}
            className="w-full"
            onClick={startGame}
            disabled={!hasEnoughPlayers}
        >
            {t('game.start')}
        </Button>
    );
}