export const GamePageState = {
    Joining: 'Joining',
    Ready: 'Ready'
} as const;

export type GamePageState = (typeof GamePageState)[keyof typeof GamePageState];