import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

import type { Card } from '@/features/game/models/Card';
import type { GameWonNotification } from '@/features/game/models/GameWonNotification';
import type { PublicGameState } from '@/features/game/models/PublicGameState';
import type { RoundResolvedNotification } from '@/features/game/models/RoundResolvedNotification';

type GameStateState = {
  gameState: PublicGameState | null;
  playerId: string | null;
  cards: Card[];
  winnerData: GameWonNotification | null;
  roundResultData: RoundResolvedNotification | null;
};

const initialState: GameStateState = {
  gameState: null,
  playerId: null,
  cards: [],
  winnerData: null,
  roundResultData: null,
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
    setWinnerData(state, action: PayloadAction<GameWonNotification>) {
      state.winnerData = action.payload;
    },
    setRoundResultData(state, action: PayloadAction<RoundResolvedNotification>) {
      state.roundResultData = action.payload;
    },
    clearRoundResultData(state) {
      state.roundResultData = null;
    },
    resetGameState(state) {
      state.gameState = null;
      state.playerId = null;
      state.cards = [];
      state.winnerData = null;
      state.roundResultData = null;
    },
  },
});

export const {
  setGameState, setCards, gameStarted, gameStateUpdated,
  setWinnerData, setRoundResultData, clearRoundResultData,
  resetGameState,
} = gameStateSlice.actions;
export default gameStateSlice.reducer;
