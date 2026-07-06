import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { Outlet, useLocation, useNavigate, useParams } from "react-router-dom";

import { supportedLangs } from "@/features/localization/consts/supportedLangs";

import { useGenericSearchParams } from "../hooks/useGenericSearchParams";
import type { LangParams } from "../models/Params";
import type { InviteQueryParams } from "../models/QueryParams";

const LanguageGuard = () => {
  const { lang } = useParams<LangParams>();
  const { i18n } = useTranslation();
  const { pathname } = useLocation();
  const [ searchParams, setSearchParams ] = useGenericSearchParams<InviteQueryParams>();
  const navigate = useNavigate();

  const isInvite = searchParams.invite === "true";
  const langIsValid = lang && supportedLangs.includes(lang);

  useEffect(() => {
    if (!langIsValid) {
      navigate("/en", { replace: true });
    }
  }, [langIsValid, navigate]);

  useEffect(() => {
    if (!langIsValid) {
      return;
    }

    if (i18n.language === lang) {
      return;
    }

    if (isInvite) {
      const segments = pathname.split('/').filter(Boolean);
      segments[0] = i18n.language;
      navigate(`/${segments.join("/")}`, { replace: true });
    }
    else {
      i18n.changeLanguage(lang);
    }
  }, [langIsValid, i18n, lang, navigate]);

  useEffect(() => {
    if (isInvite) {
      setSearchParams(prev => {
        delete prev["invite"];
        return prev;
      });
    }
  }, [isInvite, setSearchParams]);

  return <Outlet />;
};

export default LanguageGuard;
