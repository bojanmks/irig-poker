import { DynamicForm, type FieldConfig } from "@/features/form/components/DynamicForm";
import { usernameValidation } from "../gameValidationRules";
import { useCallback, useState } from "react";
import { FieldType } from "@/features/form/consts/FieldType";

const formFields: FieldConfig[] = [
    {
      name: "username",
      type: FieldType.Text,
      label: "game.enterYourName",
      validation: usernameValidation
    }
];

type HandleSubmitData = { username: string };

export function EnterNameForm({ onSubmit }: { onSubmit: (username: string) => void }) {
  const [showFormLoading, setShowFormLoading] = useState(false);

  const handleSubmit = useCallback((data: HandleSubmitData) => {
    setShowFormLoading(true);
    onSubmit(data.username);
  }, [onSubmit]);

  return (
      <DynamicForm<HandleSubmitData>
        fields={formFields}
        onSubmit={handleSubmit}
        submitLabel="game.joinGame"
        autoFocusFieldName="username"
        showLoading={showFormLoading}
      />
    )
}