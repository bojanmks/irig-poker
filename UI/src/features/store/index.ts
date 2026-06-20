import { configureStore } from '@reduxjs/toolkit';
import themeReducer from '@/features/themes/store/themeSlice';
import wrapperClassReducer from '@/features/shared/store/wrapperClassSlice';
import gameStateReducer from '@/features/game/store/gameStateSlice';

export const store = configureStore({
  reducer: {
    theme: themeReducer,
    wrapperClass: wrapperClassReducer,
    gameState: gameStateReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
