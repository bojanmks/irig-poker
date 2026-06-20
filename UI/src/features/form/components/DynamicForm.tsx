import { useForm, type FieldValues, type Path, type SubmitHandler } from "react-hook-form"
import { z, ZodType } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useTranslation } from "react-i18next"
import { Input } from "@/features/shared/components/shadcn/Input"
import { Button } from "@/features/shared/components/shadcn/Button"
import { useEffect, useRef } from "react"
import { FieldType } from "../consts/FieldType"

export type FieldConfig = {
  name: string
  type: FieldType
  label: string
  validation: ZodType
}

type DynamicFormProps<TFormData extends Record<string, unknown>> = {
  fields: FieldConfig[]
  onSubmit: (data: TFormData) => void
  submitLabel: string
  autoFocusFieldName?: string
  showLoading?: boolean
}

export function DynamicForm<TFormData extends Record<string, unknown> = Record<string, unknown>>({ fields, onSubmit, submitLabel, autoFocusFieldName, showLoading = false }: DynamicFormProps<TFormData>) {
  const { t } = useTranslation()

  const schema = z.object(
    fields.reduce((acc, field) => {
      acc[field.name] = field.validation
      return acc
    }, {} as Record<string, ZodType>)
  )

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm({
    resolver: zodResolver(schema),
  })

  const inputRefs = useRef<Record<string, HTMLInputElement | null>>({})

  useEffect(() => {
    if (autoFocusFieldName && inputRefs.current[autoFocusFieldName]) {
      inputRefs.current[autoFocusFieldName]?.focus()
    }
  }, [autoFocusFieldName])

  const renderField = (field: FieldConfig) => {
    switch (field.type) {
      case FieldType.Text:
        return (
          <div key={field.name} className="space-y-1">
            <Input
              placeholder={t(field.label)}
              {...register(field.name as Path<FieldValues>)}
              ref={(el) => {
                register(field.name as Path<FieldValues>).ref(el)
                inputRefs.current[field.name] = el
              }}
            />
            {errors[field.name]?.message && (
              <p className="text-sm text-destructive">
                {t(String(errors[field.name]?.message))}
              </p>
            )}
          </div>
        )
      default:
        return null
    }
  }

  return (
    <form onSubmit={handleSubmit(onSubmit as SubmitHandler<FieldValues>)} className="space-y-4">
      {fields.map(renderField)}
      <Button type="submit" loading={isSubmitting || showLoading} className="w-full">
        {t(submitLabel)}
      </Button>
    </form>
  )
}
