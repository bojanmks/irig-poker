import { useCallback,useMemo } from "react";
import { useTranslation } from "react-i18next";
import { useLocation, useNavigate } from "react-router-dom";

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
} from "@/features/shared/components/shadcn/Select";

function replaceLanguage(pathname: string, lang: string) {
  return pathname.replace(/^\/[^/]+/, `/${lang}`);
}

const languages = [
  { code: "sr", label: "SR", flag: "rs" },
  { code: "en", label: "EN", flag: "gb" }
] as const;

const LanguageSwitcher = () => {
  const { i18n } = useTranslation();
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const handleChange = useCallback(
    (newLang: string) => {
      i18n.changeLanguage(newLang);
      navigate(replaceLanguage(pathname, newLang), {
        replace: true,
      });
    },
    [pathname, navigate, i18n]
  );

  const selected = useMemo(() => {
    return languages.find(lang => lang.code === i18n.language) || languages.find(lang => lang.code === 'en');
  }, [languages, i18n.language]);

  return (
    <Select
        value={i18n.language}
        onValueChange={handleChange}
    >
      <SelectTrigger>
        <span>{selected?.label}</span>
      </SelectTrigger>
      <SelectContent>
        {languages.map((lang) => (
          <SelectItem value={lang.code} key={lang.code} aria-label={lang.label}>
            <span>{lang.label}</span>
          </SelectItem>
        ))}
      </SelectContent>
    </Select>
  );
};

export default LanguageSwitcher;
