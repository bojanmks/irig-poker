import { useEffect } from "react";

import { setAdditionalClass } from "@/features/shared/store/wrapperClassSlice";
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";

import { GamePageState } from "../consts/GamePageState";

export function useGamePageWrapperClass(
    gamePageState: GamePageState
) {
    const dispatch = useAppDispatch()
    const gameState = useAppSelector((state) => state.gameState.gameState);
    
    useEffect(() => {
        const additionalClass = {
            [GamePageState.Connecting]: 'max-w-md',
            [GamePageState.Joining]: 'max-w-md',
            [GamePageState.EnterNameToJoin]: 'max-w-md',
            [GamePageState.Ready]: gameState?.hasStarted ? 'w-full' : 'max-w-md',
        }[gamePageState] || '';

        dispatch(setAdditionalClass(additionalClass))

        return () => {
            dispatch(setAdditionalClass(""))
        }
    }, [dispatch, gamePageState, gameState?.hasStarted])
}