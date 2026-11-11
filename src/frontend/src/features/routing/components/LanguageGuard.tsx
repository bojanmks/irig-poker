import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { Outlet, useLocation, useNavigate, useParams } from "react-router-dom";

import { supportedLangs } from "@/features/localization/consts/supportedLangs";
import type { Language } from "@/features/localization/types/Language";

import { QueryParam } from "../consts/QueryParam";
import { useGenericSearchParams } from "../hooks/useGenericSearchParams";
import type { LangParams } from "../models/Params";
import type { InviteQueryParams } from "../models/QueryParams";

const LanguageGuard = () => {
  const { lang } = useParams<LangParams>();
  const { i18n } = useTranslation();
  const { pathname } = useLocation();
  const [ searchParams, setSearchParams ] = useGenericSearchParams<InviteQueryParams>();
  const navigate = useNavigate();
  
  const isInvite = QueryParam.Join in searchParams;
  const langIsValid = lang && supportedLangs.includes(lang);

  useEffect(() => {
    if (!langIsValid) {
      const segments = pathname.split('/').filter(Boolean);
      segments[0] = "en" satisfies Language;
      navigate(`/${segments.join("/")}`, { replace: true });
      return;
    }

    if (i18n.language !== lang) {
      if (isInvite) {
        const segments = pathname.split('/').filter(Boolean);
        segments[0] = i18n.language;
        navigate(`/${segments.join("/")}`, { replace: true });
      } else {
        i18n.changeLanguage(lang);
      }
      return;
    }

    if (isInvite) {
      setSearchParams(prev => {
        const copy = { ...prev };
        delete copy[QueryParam.Join];
        return copy;
      }, { replace: true });
    }
  }, [langIsValid, lang, pathname, i18n.language, i18n.changeLanguage, isInvite, navigate, setSearchParams]);

  return <Outlet />;
};

export default LanguageGuard;