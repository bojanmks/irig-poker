import { type Dispatch, type SetStateAction,useCallback, useEffect } from "react";

import { DynamicForm, type FieldConfig } from "@/features/form/components/DynamicForm";
import { FieldType } from "@/features/form/consts/FieldType";

import { GamePageState } from "../consts/GamePageState";
import { usernameValidation } from "../gameValidationRules";

const formFields: FieldConfig[] = [
    {
      name: "username",
      type: FieldType.Text,
      label: "game.enterYourName",
      validation: usernameValidation
    }
];

type HandleSubmitData = { username: string };

type EnterNameFormParams = {
  setUsername: Dispatch<SetStateAction<string | undefined>>;
  setPageState: Dispatch<SetStateAction<GamePageState>>;
};

export function EnterNameForm(params: EnterNameFormParams) {
  const { setUsername, setPageState } = params;

  const handleSubmit = useCallback((data: HandleSubmitData) => {
    setUsername(data.username);
  }, [setUsername]);

  useEffect(() => {
    setPageState(GamePageState.EnterNameToJoin);

    return () => {
      setPageState(GamePageState.Joining);
    }
  }, [setPageState]);

  return (
      <DynamicForm<HandleSubmitData>
        fields={formFields}
        onSubmit={handleSubmit}
        submitLabel="game.joinGame"
        autoFocusFieldName="username"
      />
    )
}