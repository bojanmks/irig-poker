import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { PublicGameState } from '@/features/game/models/PublicGameState';
import type { Player } from '@/features/game/models/Player';

type GameStateState = {
  gameState: PublicGameState | null;
};

const initialState: GameStateState = {
  gameState: null,
};

const gameStateSlice = createSlice({
  name: "gameState",
  initialState,
  reducers: {
    setGameState(state, action: PayloadAction<PublicGameState | null>) {
      state.gameState = action.payload;
    },
    playerJoined(state, action: PayloadAction<{ connectionId: string; player: Player }>) {
      if (state.gameState) {
        state.gameState.players[action.payload.connectionId] = action.payload.player;
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
    },
  },
});

export const { setGameState, playerJoined, playerLeft, gameStarted, resetGameState } = gameStateSlice.actions;
export default gameStateSlice.reducer;
