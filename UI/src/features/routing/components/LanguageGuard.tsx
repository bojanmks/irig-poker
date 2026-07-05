import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { Navigate, Outlet, useParams } from "react-router-dom";

const supportedLangs = ["en", "sr"];

const LanguageGuard = () => {
  const { lang } = useParams<{ lang: string }>();
  const { i18n } = useTranslation();

  useEffect(() => {
    if (lang && supportedLangs.includes(lang) && i18n.language !== lang) {
      i18n.changeLanguage(lang);
    }
  }, [lang, i18n]);

  if (!lang) {
    return <Navigate to="/en" replace />;
  }

  if (!supportedLangs.includes(lang)) {
    const detectedLang = i18n.language.startsWith("sr") ? "sr" : "en";
    return <Navigate to={`/${detectedLang}/${lang}`} replace />;
  }

  return <Outlet />;
};

export default LanguageGuard;
