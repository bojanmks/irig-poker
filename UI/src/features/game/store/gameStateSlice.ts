import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

import type { Player } from '@/features/game/models/Player';
import type { PublicGameState } from '@/features/game/models/PublicGameState';

type GameStateState = {
  gameState: PublicGameState | null;
  playerId: string | null;
};

const initialState: GameStateState = {
  gameState: null,
  playerId: null,
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
    playerJoined(state, action: PayloadAction<{ playerId: string; player: Player }>) {
      if (state.gameState) {
        state.gameState.players[action.payload.playerId] = action.payload.player;
      }
    },
    playerLeft(state, action: PayloadAction<string>) {
      if (state.gameState) {
        delete state.gameState.players[action.payload];
      }
    },
    gameStarted(state) {
      if (state.gameState) {
        state.gameState.hasStarted = true;
      }
    },
    resetGameState(state) {
      state.gameState = null;
      state.playerId = null;
    },
  },
});

export const { setGameState, playerJoined, playerLeft, gameStarted, resetGameState } = gameStateSlice.actions;
export default gameStateSlice.reducer;
