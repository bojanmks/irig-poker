import { readFileSync, writeFileSync, mkdirSync, existsSync } from "node:fs";
import { resolve, dirname } from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = dirname(fileURLToPath(import.meta.url));
const root = resolve(__dirname, "..");

const domain = "https://irigpoker.bojanm.dev";

function readJson(filePath) {
  return JSON.parse(readFileSync(filePath, "utf-8"));
}

function get(obj, key) {
  const parts = key.split(".");
  let val = obj;
  for (const p of parts) {
    val = val?.[p];
  }
  return typeof val === "string" ? val : key;
}

function escapeHtml(str) {
  return str.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;");
}

const locales = {
  en: readJson(resolve(root, "src/locales/en.json")),
  sr: readJson(resolve(root, "src/locales/sr.json")),
};

const template = readFileSync(resolve(root, "dist/index.html"), "utf-8");

for (const [lang, locale] of Object.entries(locales)) {
  const title = escapeHtml(get(locale, "meta.title"));
  const description = escapeHtml(get(locale, "meta.description"));
  const keywords = escapeHtml(get(locale, "meta.keywords"));
  const aboutHeading = escapeHtml(get(locale, "about.heading"));
  const aboutSubtitle = escapeHtml(get(locale, "about.subtitle"));
  const aboutDescription = escapeHtml(get(locale, "about.description"));
  const howToPlay = escapeHtml(get(locale, "about.howToPlay"));
  const rules = locale.about?.rules ?? [];
  const note = escapeHtml(get(locale, "about.note"));

  const jsonLd = JSON.stringify({
    "@context": "https://schema.org",
    "@type": "WebApplication",
    name: title,
    description,
    url: `${domain}/${lang}`,
    applicationCategory: "GameApplication",
    operatingSystem: "Web",
    inLanguage: ["en", "sr"],
    offers: { "@type": "Offer", price: "0", priceCurrency: "USD" },
  });

  const headTags = `
    <meta name="description" content="${description}" />
    <meta name="keywords" content="${keywords}" />
    <meta property="og:title" content="${title}" />
    <meta property="og:description" content="${description}" />
    <meta property="og:image" content="${domain}/og-image.png" />
    <meta property="og:url" content="${domain}/${lang}" />
    <meta property="og:type" content="website" />
    <meta name="twitter:card" content="summary_large_image" />
    <meta name="twitter:title" content="${title}" />
    <meta name="twitter:description" content="${description}" />
    <meta name="twitter:image" content="${domain}/og-image.png" />
    <link rel="canonical" href="${domain}/${lang}" />
    <link rel="alternate" hrefLang="en" href="${domain}/en" />
    <link rel="alternate" hrefLang="sr" href="${domain}/sr" />
    <link rel="alternate" hrefLang="x-default" href="${domain}/en" />
    <script type="application/ld+json">${jsonLd}</script>
  `;

  const fallbackContent = `<h1>${title}</h1><p>${description}</p><h2>${aboutHeading}</h2><p>${aboutSubtitle}</p><p>${aboutDescription}</p><h3>${howToPlay}</h3><ol>${rules.map(r => `<li>${escapeHtml(r)}</li>`).join("")}</ol><p>${note}</p>`;

  let html = template
    .replace('<html lang="en">', `<html lang="${lang}">`)
    .replace("<title>Irig Poker</title>", `<title>${title}</title>`)
    .replace("</head>", `${headTags}\n  </head>`)
    .replace('<div id="root"></div>', `<div id="root">${fallbackContent}</div>`);

  const outDir = resolve(root, "dist", lang);
  if (!existsSync(outDir)) {
    mkdirSync(outDir, { recursive: true });
  }
  writeFileSync(resolve(outDir, "index.html"), html, "utf-8");

  console.log(`✅ Prerendered /${lang}/index.html`);
}
