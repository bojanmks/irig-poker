import { useCallback } from "react";

import type { HubNotification } from "@/features/http/models/HubNotification";
import { useAppDispatch } from "@/features/store/hooks";

import type { CardsDealtNotification } from "../models/CardsDealtNotification";
import { setCards } from "../store/gameStateSlice";

export function useDealingEventHandlers() {
    const dispatch = useAppDispatch();

    const onCardsDealt = useCallback((notification: HubNotification<CardsDealtNotification>) => {
        dispatch(setCards(notification.data.cards));
    }, [dispatch]);

    return { onCardsDealt };
}