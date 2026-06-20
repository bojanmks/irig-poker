import { useNavigate } from "react-router-dom"
import { useRequest } from "@/features/http/hooks/useRequest"
import { DynamicForm, type FieldConfig } from "@/features/form/components/DynamicForm"
import { usernameValidation } from "@/features/game/gameValidationRules";
import { useCallback, useEffect } from "react";
import { FieldType } from "@/features/form/consts/FieldType";
import { useAppDispatch } from "@/features/store/hooks";
import { setAdditionalClass } from "@/features/shared/store/wrapperClassSlice";

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
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(setAdditionalClass("max-w-md"))

    return () => {
      dispatch(setAdditionalClass(""))
    }
  }, [dispatch])

  const handleSubmit = useCallback(async (data: { username: string }) => {
    const result = await send<void, string>({
      method: "POST",
      url: "/games/create",
    })

    navigate(`/${result.data}`, {
      state: { username: data.username }
    })
  },[navigate, send]);

  return (
    <DynamicForm<{ username: string }>
      fields={formFields}
      onSubmit={handleSubmit}
      submitLabel="game.createPrivateGame"
      autoFocusFieldName="username"
    />
  )
}
