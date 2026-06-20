import type { FieldErrors } from "./FieldError"

export interface EndpointResponse<T> {
  data: T;
  errorMessages: string[];
  fieldErrors: FieldErrors[];
  statusCode: number;
  isSuccess: boolean;
}