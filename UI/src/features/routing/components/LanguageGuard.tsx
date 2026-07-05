import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { Navigate, Outlet, useLocation, useParams } from "react-router-dom";

import { supportedLangs } from "@/features/localization/consts/supportedLangs";
import type { Language } from "@/features/localization/types/Language";

const LanguageGuard = () => {
  const { lang } = useParams<{ lang: Language }>();
  const { i18n } = useTranslation();
  const { pathname } = useLocation();

  useEffect(() => {
    if (lang && supportedLangs.includes(lang) && i18n.language !== lang) {
      i18n.changeLanguage(lang);
    }
  }, [lang, i18n]);

  if (!lang || !supportedLangs.includes(lang)) {
    const segments = pathname.split('/').filter(Boolean);
    segments[0] = i18n.language;
    return <Navigate to={`/${segments.join("/")}`} replace />;
  }

  return <Outlet />;
};

export default LanguageGuard;
