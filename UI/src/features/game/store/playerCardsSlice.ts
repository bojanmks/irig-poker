import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

import type { Card } from '@/features/game/models/Card';

type PlayerCardsState = {
  cards: Card[];
};

const initialState: PlayerCardsState = {
  cards: [],
};

const playerCardsSlice = createSlice({
  name: "playerCards",
  initialState,
  reducers: {
    setCards(state, action: PayloadAction<Card[]>) {
      state.cards = action.payload;
    },
    addCard(state, action: PayloadAction<Card>) {
      state.cards.push(action.payload);
    },
    resetCards(state) {
      state.cards = [];
    },
  },
});

export const { setCards, addCard, resetCards } = playerCardsSlice.actions;
export default playerCardsSlice.reducer;
