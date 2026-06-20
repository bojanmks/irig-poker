export const GamePageState = {
    None: 1,
    EnterNameToJoin: 2,
    Ready: 3
} as const;

export type GamePageState = (typeof GamePageState)[keyof typeof GamePageState];