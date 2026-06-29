import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

import type { Card } from '@/features/game/models/Card';
import type { Player } from '@/features/game/models/Player';
import type { PublicGameState } from '@/features/game/models/PublicGameState';
import type { RoundResolvedNotification } from '@/features/game/models/RoundResolvedNotification';

type GameStateState = {
  gameState: PublicGameState | null;
  playerId: string | null;
  cards: Card[];
  winner: { winnerPlayerId: string; winnerUsername: string } | null;
  roundResult: RoundResolvedNotification | null;
  eliminatedPlayerId: string | null;
};

const initialState: GameStateState = {
  gameState: null,
  playerId: null,
  cards: [],
  winner: null,
  roundResult: null,
  eliminatedPlayerId: null,
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
    setCards(state, action: PayloadAction<Card[]>) {
      state.cards = action.payload;
    },
    gameStarted(state, action: PayloadAction<PublicGameState>) {
      state.gameState = action.payload;
      state.roundResult = null;
      state.eliminatedPlayerId = null;
    },
    gameWon(state, action: PayloadAction<{ winnerPlayerId: string; winnerUsername: string }>) {
      state.winner = action.payload;
    },
    gameStateUpdated(state, action: PayloadAction<PublicGameState>) {
      state.gameState = action.payload;
    },
    claimMade(state) {
      if (state.gameState) {
        state.roundResult = null;
      }
    },
    roundResolved(state, action: PayloadAction<RoundResolvedNotification>) {
      state.roundResult = action.payload;
    },
    playerEliminated(state, action: PayloadAction<string>) {
      state.eliminatedPlayerId = action.payload;
    },
    resetGameState(state) {
      state.gameState = null;
      state.playerId = null;
      state.cards = [];
      state.winner = null;
      state.roundResult = null;
      state.eliminatedPlayerId = null;
    },
  },
});

export const { setGameState, playerJoined, playerLeft, adminChanged, setCards, gameStarted, gameWon, gameStateUpdated, claimMade, roundResolved, playerEliminated, resetGameState } = gameStateSlice.actions;
export default gameStateSlice.reducer;
