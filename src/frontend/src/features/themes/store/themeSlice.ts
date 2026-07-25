import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

type Theme = "dark" | "light" | "system";

type ThemeState = {
  theme: Theme;
};

const getInitialTheme = (): Theme => {
  return (localStorage.getItem("vite-ui-theme") as Theme) || "system";
};

const initialState: ThemeState = {
  theme: getInitialTheme(),
};

const themeSlice = createSlice({
  name: "theme",
  initialState,
  reducers: {
    setTheme(state, action: PayloadAction<Theme>) {
      state.theme = action.payload;
      localStorage.setItem("vite-ui-theme", action.payload);
    },
  },
});

export const { setTheme } = themeSlice.actions;
export type { Theme };
export default themeSlice.reducer;
