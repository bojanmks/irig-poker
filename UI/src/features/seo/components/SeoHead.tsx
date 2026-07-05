import { Helmet } from "react-helmet-async";
import { useTranslation } from "react-i18next";
import { useLocation } from "react-router-dom";

interface SeoHeadProps {
  titleKey: string;
  descriptionKey: string;
}

const SeoHead = ({ titleKey, descriptionKey }: SeoHeadProps) => {
  const { t, i18n } = useTranslation();
  const { pathname } = useLocation();

  const currentLang = i18n.language.startsWith("sr") ? "sr" : "en";
  const otherLang = currentLang === "en" ? "sr" : "en";

  const alternatePath = pathname.replace(`/${currentLang}`, `/${otherLang}`);
  const origin = window.location.origin;
  const currentUrl = origin + pathname;

  const jsonLd = {
    "@context": "https://schema.org",
    "@type": "WebApplication",
    name: t(titleKey),
    description: t(descriptionKey),
    url: currentUrl,
    applicationCategory: "GameApplication",
    operatingSystem: "Web",
    inLanguage: [ "en", "sr" ],
    offers: {
      "@type": "Offer",
      price: "0",
      priceCurrency: "USD",
    },
  };

  return (
    <Helmet>
      <html lang={currentLang} />
      <title>{t(titleKey)}</title>
      <meta name="description" content={t(descriptionKey)} />
      <meta property="og:title" content={t(titleKey)} />
      <meta property="og:description" content={t(descriptionKey)} />
      <meta property="og:image" content={`${origin}/og-image.png`} />
      <meta property="og:url" content={currentUrl} />
      <meta property="og:type" content="website" />
      <meta name="twitter:card" content="summary_large_image" />
      <meta name="twitter:title" content={t(titleKey)} />
      <meta name="twitter:description" content={t(descriptionKey)} />
      <meta name="twitter:image" content={`${origin}/og-image.png`} />
      <link rel="alternate" hrefLang={currentLang} href={currentUrl} />
      <link rel="alternate" hrefLang={otherLang} href={origin + alternatePath} />
      <link rel="alternate" hrefLang="x-default" href={`${origin}/en`} />
      <script type="application/ld+json">{JSON.stringify(jsonLd)}</script>
    </Helmet>
  );
};

export default SeoHead;
