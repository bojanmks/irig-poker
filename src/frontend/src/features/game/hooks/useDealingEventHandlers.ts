import { useCallback } from "react";

import cardsDealtSound from "@/assets/cards_dealt.mp3";
import type { HubNotification } from "@/features/http/models/HubNotification";
import { playSound } from "@/features/shared/utils/audio";
import { useAppDispatch } from "@/features/store/hooks";

import type { CardsDealtNotification } from "../models/CardsDealtNotification";
import { setCards } from "../store/gameStateSlice";

export function useDealingEventHandlers() {
    const dispatch = useAppDispatch();

    const onCardsDealt = useCallback((notification: HubNotification<CardsDealtNotification>) => {
        playSound(cardsDealtSound);
        dispatch(setCards(notification.data.cards));
    }, [dispatch]);

    return { onCardsDealt };
}