import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

import type { GameWonNotification } from '@/features/game/models/GameWonNotification';
import type { Player } from '@/features/game/models/Player';
import type { PublicGameState } from '@/features/game/models/PublicGameState';

type GameStateState = {
  gameState: PublicGameState | null;
  playerId: string | null;
  winner: GameWonNotification | null;
};

const initialState: GameStateState = {
  gameState: null,
  playerId: null,
  winner: null,
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
    playerJoined(state, action: PayloadAction<{ player: Player, playerOrder: string[] }>) {
      if (state.gameState) {
        state.gameState.players[action.payload.player.playerId] = action.payload.player;
        state.gameState.playerOrder = action.payload.playerOrder;
      }
    },
    playerLeft(state, action: PayloadAction<string>) {
      if (state.gameState) {
        delete state.gameState.players[action.payload];
        state.gameState.playerOrder = state.gameState.playerOrder.filter(id => id !== action.payload);
      }
    },
    adminChanged(state, action: PayloadAction<string>) {
      if (state.gameState) {
        const player = state.gameState.players[action.payload];
        if (player) {
          player.isAdmin = true;
        }
      }
    },
    gameStarted(state, action: PayloadAction<PublicGameState>) {
      state.gameState = action.payload;
    },
    gameWon(state, action: PayloadAction<GameWonNotification>) {
      state.winner = action.payload;
    },
    resetGameState(state) {
      state.gameState = null;
      state.playerId = null;
      state.winner = null;
    },
  },
});

export const { setGameState, playerJoined, playerLeft, adminChanged, gameStarted, gameWon, resetGameState } = gameStateSlice.actions;
export default gameStateSlice.reducer;
