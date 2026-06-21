import { type Dispatch, type SetStateAction,useCallback } from "react";

import { DynamicForm, type FieldConfig } from "@/features/form/components/DynamicForm";
import { FieldType } from "@/features/form/consts/FieldType";

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
  setUsername: Dispatch<SetStateAction<string | undefined>>
};

export function EnterNameForm(params: EnterNameFormParams) {
  const { setUsername } = params;

  const handleSubmit = useCallback((data: HandleSubmitData) => {
    setUsername(data.username);
  }, [setUsername]);

  return (
      <DynamicForm<HandleSubmitData>
        fields={formFields}
        onSubmit={handleSubmit}
        submitLabel="game.joinGame"
        autoFocusFieldName="username"
      />
    )
}