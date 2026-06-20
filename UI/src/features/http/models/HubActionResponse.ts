import type { FieldErrors } from "./FieldError";

export interface HubActionResponse<T> {
    data: T | null;
    errors: string[] | null;
    fieldErrors: FieldErrors[] | null;
    isSuccess: boolean;
}