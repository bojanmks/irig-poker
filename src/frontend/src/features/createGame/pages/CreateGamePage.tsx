import { useCallback, useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom"

import { CircleQuestionMark } from "lucide-react";

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
  const { t, i18n } = useTranslation();

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

    navigate(`/${i18n.language}/${result.data}`, {
      state: { username: data.username }
    })
  },[navigate, send, i18n.language]);

  return (
    <>
      <SeoHead titleKey="meta.title" descriptionKey="meta.description" keywordsKey="meta.keywords" />
      <DynamicForm<{ username: string }>
        fields={formFields}
        onSubmit={handleSubmit}
        submitLabel="game.createPrivateGame"
        autoFocusFieldName="username"
      />
      <section className="mt-10 space-y-4 text-sm text-muted-foreground">
        <div>
          <div className="flex items-center gap-2">
            <CircleQuestionMark />
            <h2 className="text-lg font-semibold text-foreground">{t("about.heading")}</h2>
          </div>
          <p className="mt-1 leading-relaxed">{t("about.subtitle")}</p>
          <p className="mt-2 leading-relaxed">{t("about.description")}</p>
        </div>
        <div>
          <h3 className="font-semibold text-foreground">{t("about.howToPlay")}</h3>
          <ol className="mt-2 list-inside list-decimal space-y-1">
            {(t("about.rules", { returnObjects: true }) as string[]).map((rule, i) => (
              <li key={i}>{rule}</li>
            ))}
          </ol>
        </div>
        <span className="text-xs">{t("about.note")}</span>
      </section>
    </>
  )
}
