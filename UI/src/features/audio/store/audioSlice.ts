import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

type AudioState = {
  muted: boolean;
};

const initialState: AudioState = {
  muted: localStorage.getItem("audio-muted") === "true",
};

const audioSlice = createSlice({
  name: "audio",
  initialState,
  reducers: {
    setMuted(state, action: PayloadAction<boolean>) {
      state.muted = action.payload;
      localStorage.setItem("audio-muted", String(action.payload));
    },
    toggleMuted(state) {
      state.muted = !state.muted;
      localStorage.setItem("audio-muted", String(state.muted));
    },
  },
});

export const { setMuted, toggleMuted } = audioSlice.actions;
export default audioSlice.reducer;
