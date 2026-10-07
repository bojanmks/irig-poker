import { useMemo } from "react";
import { useTranslation } from "react-i18next";
import { Link, useLocation } from "react-router-dom";

import { supportedLangs } from "@/features/localization/consts/supportedLangs";
import type { Language } from "@/features/localization/types/Language";
import { buttonVariants } from "@/features/shared/components/shadcn/Button";
import { cn } from "@/lib/utils";

function replaceLanguage(pathname: string, lang: string) {
  return pathname.replace(/^\/[^/]+/, `/${lang}`);
}

const languages = [
  { code: "sr", label: "SR", flag: "rs" },
  { code: "en", label: "EN", flag: "gb" }
] as const;

const LanguageSwitcher = () => {
  const { i18n } = useTranslation();
  const { pathname } = useLocation();

  const currentLang = useMemo(() => {
    const segment = pathname.split("/").filter(Boolean)[0];
    return supportedLangs.includes(segment as Language) ? segment : "en";
  }, [pathname]);

  return (
    <div className="flex items-center gap-1">
      {languages.map((lang) => (
        <Link
          key={lang.code}
          to={replaceLanguage(pathname, lang.code)}
          onClick={() => i18n.changeLanguage(lang.code)}
          aria-current={lang.code === currentLang ? "true" : undefined}
          className={cn(
            buttonVariants({ variant: "ghost", size: "sm" }),
            "px-2",
            lang.code === currentLang && "bg-accent text-accent-foreground"
          )}
        >
          {lang.label}
        </Link>
      ))}
    </div>
  );
};

export default LanguageSwitcher;
