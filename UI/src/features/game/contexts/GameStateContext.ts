import { createContext, useContext, type Dispatch, type SetStateAction } from "react";
import type { PublicGameState } from "../models/PublicGameState";

export type GameStateContextType = {
  gameState: PublicGameState | null;
  setGameState: Dispatch<SetStateAction<PublicGameState | null>>;
};

export const GameStateContext = createContext<GameStateContextType | undefined>(undefined);

export function useGameState() {
  const ctx = useContext(GameStateContext);
  if (!ctx) throw new Error("useGameState must be used within a GameStateContext.Provider");
  return ctx;
}