import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

type WrapperClassState = {
  additionalClass: string;
};

const initialState: WrapperClassState = {
  additionalClass: "",
};

const wrapperClassSlice = createSlice({
  name: "wrapperClass",
  initialState,
  reducers: {
    setAdditionalClass(state, action: PayloadAction<string>) {
      state.additionalClass = action.payload;
    },
  },
});

export const { setAdditionalClass } = wrapperClassSlice.actions;
export default wrapperClassSlice.reducer;
