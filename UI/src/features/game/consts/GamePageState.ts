export const GamePageState = {
    Connecting: 'Connecting',
    Joining: 'Joining',
    EnterNameToJoin: 'EnterNameToJoin',
    Ready: 'Ready'
} as const;

export type GamePageState = (typeof GamePageState)[keyof typeof GamePageState];