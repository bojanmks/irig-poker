import { useForm } from "react-hook-form"
import { z, ZodType } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useTranslation } from "react-i18next"
import { Input } from "@/features/shared/components/Input"
import { Button } from "@/features/shared/components/Button"
import { useEffect, useRef } from "react"
import { FieldType } from "../consts/FieldType"

export type FieldConfig = {
  name: string
  type: FieldType
  label: string
  validation: ZodType
}

type DynamicFormProps = {
  fields: FieldConfig[]
  onSubmit: (data: any) => void
  submitLabel: string
  autoFocusFieldName?: string
  showLoading?: boolean
}

export function DynamicForm({ fields, onSubmit, submitLabel, autoFocusFieldName, showLoading = false }: DynamicFormProps) {
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
              {...register(field.name)}
              ref={(el) => {
                register(field.name).ref(el) // for react-hook-form
                inputRefs.current[field.name] = el // for our manual focus
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
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      {fields.map(renderField)}
      <Button type="submit" loading={isSubmitting || showLoading} className="w-full">
        {t(submitLabel)}
      </Button>
    </form>
  )
}