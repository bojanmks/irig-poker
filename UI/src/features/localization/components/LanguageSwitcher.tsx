import { useTranslation } from "react-i18next";
import { useMemo } from "react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
} from "@/features/shared/components/shadcn/Select";

const LanguageSwitcher = () => {
  const { i18n, t } = useTranslation();
  const currentLang = i18n.language.startsWith("sr") ? "sr" : "en";

  const languages = useMemo(() => {
    return [
      { code: "sr", label: "SR", imageAlt: t('serbianLanguage'), flag: "rs" },
      { code: "en", label: "EN", imageAlt: t('englishLanguage'), flag: "gb" }
    ];
  }, [t]);

  const handleChange = (lang: string) => {
    i18n.changeLanguage(lang);
  };

  const selected = languages.find((lang) => lang.code === currentLang);

  return (
    <Select
        value={currentLang}
        onValueChange={handleChange}
    >
      <SelectTrigger>
        <div className="flex items-center gap-2">
          <img
            src={`https://flagcdn.com/w20/${selected?.flag}.png`}
            alt={selected?.imageAlt}
            className="rounded-full w-4 h-4"
          />
          <span>{selected?.label}</span>
        </div>
      </SelectTrigger>
      <SelectContent>
        {languages.map((lang) => (
          <SelectItem value={lang.code} key={lang.code}>
            <div className="flex items-center gap-2">
              <img
                src={`https://flagcdn.com/w20/${lang.flag}.png`}
                alt={lang.imageAlt}
                className="rounded-full w-4 h-4"
              />
              <span>{lang.label}</span>
            </div>
          </SelectItem>
        ))}
      </SelectContent>
    </Select>
  );
};

export default LanguageSwitcher;
