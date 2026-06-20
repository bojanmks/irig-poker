import { useNavigate } from "react-router-dom"
import { useRequest } from "@/features/http/hooks/useRequest"
import { DynamicForm, type FieldConfig } from "@/features/form/components/DynamicForm"
import { usernameValidation } from "@/features/game/gameValidationRules";
import { useCallback, useEffect } from "react";
import { useWrapperClass } from "@/features/shared/contexts/WrapperClassContext";
import { FieldType } from "@/features/form/consts/FieldType";

const formFields: FieldConfig[] = [
  {
    name: "username",
    type: FieldType.Text,
    label: "game.enterYourName",
    validation: usernameValidation
  }
];

export function CreateGamePage() {
  const navigate = useNavigate();
  const { send } = useRequest();
  const { setAdditionalClass } = useWrapperClass();

  useEffect(() => {
    setAdditionalClass("max-w-md")

    return () => {
      setAdditionalClass("")
    }
  }, [setAdditionalClass])

  const handleSubmit = useCallback(async (data: { username: string }) => {
    const result = await send<void, string>({
      method: "POST",
      url: "/games/create",
    })

    navigate(`/${result.data}`, {
      state: { username: data.username }
    })
  },[]);

  return (
    <DynamicForm
      fields={formFields}
      onSubmit={handleSubmit}
      submitLabel="game.createPrivateGame"
      autoFocusFieldName="username"
    />
  )
}
