export const FieldType = {
    Text: 1
} as const;

export type FieldType = (typeof FieldType)[keyof typeof FieldType];