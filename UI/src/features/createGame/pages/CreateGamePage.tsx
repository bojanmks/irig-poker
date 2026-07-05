import { useCallback, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom"

import { DynamicForm, type FieldConfig } from "@/features/form/components/DynamicForm"
import { FieldType } from "@/features/form/consts/FieldType";
import { usernameValidation } from "@/features/game/gameValidationRules";
import { useRequest } from "@/features/http/hooks/useRequest"
import SeoHead from "@/features/seo/components/SeoHead"
import { setAdditionalClass } from "@/features/shared/store/wrapperClassSlice";
import { useAppDispatch } from "@/features/store/hooks";

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
  const { lang } = useParams();

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

    navigate(`/${lang}/${result.data}`, {
      state: { username: data.username }
    })
  },[navigate, send, lang]);

  return (
    <>
      <SeoHead titleKey="appTitle" descriptionKey="metaDescription" />
      <DynamicForm<{ username: string }>
        fields={formFields}
        onSubmit={handleSubmit}
        submitLabel="game.createPrivateGame"
        autoFocusFieldName="username"
      />
    </>
  )
}
