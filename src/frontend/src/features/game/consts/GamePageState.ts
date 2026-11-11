export const GamePageState = {
    Connecting: 'Connecting',
    Checking: 'Checking',
    Joining: 'Joining',
    EnterNameToJoin: 'EnterNameToJoin',
    Ready: 'Ready'
} as const;

export type GamePageState = (typeof GamePageState)[keyof typeof GamePageState];