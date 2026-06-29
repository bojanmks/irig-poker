import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

import type { Card } from '@/features/game/models/Card';
import type { PublicGameState } from '@/features/game/models/PublicGameState';

type GameStateState = {
  gameState: PublicGameState | null;
  playerId: string | null;
  cards: Card[];
};

const initialState: GameStateState = {
  gameState: null,
  playerId: null,
  cards: [],
};

const gameStateSlice = createSlice({
  name: "gameState",
  initialState,
  reducers: {
    setGameState(state, action: PayloadAction<{ gameState: PublicGameState; playerId: string } | null>) {
      if (action.payload === null) {
        state.gameState = null;
        state.playerId = null;
      } else {
        state.gameState = action.payload.gameState;
        state.playerId = action.payload.playerId;
      }
    },
    setCards(state, action: PayloadAction<Card[]>) {
      state.cards = action.payload;
    },
    gameStarted(state, action: PayloadAction<PublicGameState>) {
      state.gameState = action.payload;
    },
    gameStateUpdated(state, action: PayloadAction<PublicGameState>) {
      state.gameState = action.payload;
    },
    resetGameState(state) {
      state.gameState = null;
      state.playerId = null;
      state.cards = [];
    },
  },
});

export const { setGameState, setCards, gameStarted, gameStateUpdated, resetGameState } = gameStateSlice.actions;
export default gameStateSlice.reducer;
