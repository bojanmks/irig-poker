import { useCallback, useMemo } from "react";
import { Helmet } from "react-helmet-async";
import { useTranslation } from "react-i18next";
import { useLocation } from "react-router-dom";

import { supportedLangs } from "@/features/localization/consts/supportedLangs";
import type { Language } from "@/features/localization/types/Language";

interface SeoHeadProps {
  titleKey: string;
  descriptionKey: string;
  keywordsKey?: string;
  robots?: string;
}

const SeoHead = ({ titleKey, descriptionKey, keywordsKey, robots }: SeoHeadProps) => {
  const { t, i18n } = useTranslation();
  const { pathname } = useLocation();

  const cleanPathname = useMemo(() => pathname.replace(/\/+$/, "") || "/", [pathname]);

  const pageLang = useMemo(() => {
    const segment = cleanPathname.split("/").filter(Boolean)[0];

    if (segment && supportedLangs.includes(segment as Language)) {
      return segment as Language;
    }

    return supportedLangs.includes(i18n.language as Language) ? (i18n.language as Language) : "en";
  }, [cleanPathname, i18n.language]);

  const otherLangs = useMemo(() => {
    return supportedLangs.filter(lang => lang !== pageLang);
  }, [pageLang]);

  const getOtherLangPath = useCallback(
    (otherLang: Language) => {
      const segments = cleanPathname.split("/");

      if (supportedLangs.includes(segments[1] as Language)) {
        segments[1] = otherLang;
      }

      return segments.join("/");
    },
    [cleanPathname]
  );

  const origin = window.location.origin;
  const currentUrl = useMemo(() => origin + cleanPathname, [origin, cleanPathname]);

  return (
    <Helmet>
      <title>{t(titleKey)}</title>
      <html lang={pageLang} />
      <meta name="description" content={t(descriptionKey)} />
      {robots && <meta name="robots" content={robots} />}
      {keywordsKey && <meta name="keywords" content={t(keywordsKey)} />}
      <meta property="og:title" content={t(titleKey)} />
      <meta property="og:description" content={t(descriptionKey)} />
      <meta property="og:image" content={`${origin}/og-image.png`} />
      <meta property="og:url" content={currentUrl} />
      <meta property="og:type" content="website" />
      <meta name="twitter:card" content="summary_large_image" />
      <meta name="twitter:title" content={t(titleKey)} />
      <meta name="twitter:description" content={t(descriptionKey)} />
      <meta name="twitter:image" content={`${origin}/og-image.png`} />
      <link rel="canonical" href={currentUrl} />
      <link rel="alternate" hrefLang={pageLang} href={currentUrl} />
      {otherLangs.map(otherLang => (<link key={otherLang} rel="alternate" hrefLang={otherLang} href={origin + getOtherLangPath(otherLang)} />))}
      <link rel="alternate" hrefLang="x-default" href={`${origin}/en`} />
    </Helmet>
  );
};

export default SeoHead;
