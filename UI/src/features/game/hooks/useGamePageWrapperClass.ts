import { useWrapperClass } from "@/features/shared/contexts/WrapperClassContext";
import { useEffect } from "react";
import { GamePageState } from "../consts/GamePageState";
import { useGameState } from "../contexts/GameStateContext";

export function useGamePageWrapperClass(
    gamePageState: GamePageState
) {
    const { setAdditionalClass } = useWrapperClass()
    const { gameState } = useGameState();
    
    useEffect(() => {
        const additionalClass = {
            [GamePageState.None]: 'max-w-md',
            [GamePageState.EnterNameToJoin]: 'max-w-md',
            [GamePageState.Ready]: gameState?.hasStarted ? 'max-w-xl' : 'max-w-md',
        }[gamePageState] || '';

    setAdditionalClass(additionalClass)

    return () => {
        setAdditionalClass("")
    }
    }, [setAdditionalClass, gamePageState, gameState?.hasStarted])
}