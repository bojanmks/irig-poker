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
}

const SeoHead = ({ titleKey, descriptionKey, keywordsKey }: SeoHeadProps) => {
  const { t, i18n } = useTranslation();
  const { pathname } = useLocation();

  const otherLangs = useMemo(() => {
    return supportedLangs.filter(lang => lang !== i18n.language);
  }, [i18n.language]);

  const getOtherLangPath = useCallback(
    (otherLang: Language) => {
      const segments = pathname.split("/");

      if (supportedLangs.includes(segments[1] as Language)) {
        segments[1] = otherLang;
      }

      return segments.join("/");
    },
    [pathname]
  );

  const origin = window.location.origin;
  const currentUrl = useMemo(() => origin + pathname, [origin, pathname]);

  const jsonLd = useMemo(() => ({
    "@context": "https://schema.org",
    "@type": "WebApplication",
    name: t(titleKey),
    description: t(descriptionKey),
    url: currentUrl,
    applicationCategory: "GameApplication",
    operatingSystem: "Web",
    inLanguage: supportedLangs,
    offers: {
      "@type": "Offer",
      price: "0",
      priceCurrency: "USD",
    },
  }), [currentUrl]);

  return (
    <Helmet>
      <html lang={i18n.language} />
      <title>{t(titleKey)}</title>
      <meta name="description" content={t(descriptionKey)} />
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
      <link rel="alternate" hrefLang={i18n.language} href={currentUrl} />
      {otherLangs.map(otherLang => (<link rel="alternate" hrefLang={otherLang} href={origin + getOtherLangPath(otherLang)} />))}
      <link rel="alternate" hrefLang="x-default" href={`${origin}/en`} />
      <script type="application/ld+json">{JSON.stringify(jsonLd)}</script>
    </Helmet>
  );
};

export default SeoHead;
