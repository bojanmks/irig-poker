import { configureStore } from '@reduxjs/toolkit';

import audioReducer from '@/features/audio/store/audioSlice';
import gameStateReducer from '@/features/game/store/gameStateSlice';
import wrapperClassReducer from '@/features/shared/store/wrapperClassSlice';
import themeReducer from '@/features/themes/store/themeSlice';

export const store = configureStore({
  reducer: {
    theme: themeReducer,
    wrapperClass: wrapperClassReducer,
    gameState: gameStateReducer,
    audio: audioReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
