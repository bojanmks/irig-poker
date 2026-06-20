import z from "zod";

export const usernameValidation = z
  .string()
  .min(1, "form.required")
  .max(20, "form.tooLong");