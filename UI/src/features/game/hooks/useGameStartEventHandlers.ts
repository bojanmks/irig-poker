import { useCallback } from "react";

import type { CardsDealtNotification } from "@/features/game/models/CardsDealtNotification";
import type { PublicGameState } from "@/features/game/models/PublicGameState";
import { gameStarted, setCards } from "@/features/game/store/gameStateSlice";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { useAppDispatch } from "@/features/store/hooks";

export function useGameStartEventHandlers() {
    const dispatch = useAppDispatch();

    const onGameStarted = useCallback((notification: HubNotification<PublicGameState>) => {
        dispatch(gameStarted(notification.data));
    }, [dispatch]);

    const onCardsDealt = useCallback((notification: HubNotification<CardsDealtNotification>) => {
        dispatch(setCards(notification.data.cards));
    }, [dispatch]);

    return { onGameStarted, onCardsDealt };
}