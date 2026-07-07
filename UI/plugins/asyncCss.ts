import type { Plugin } from "vite";

export function asyncCssPlugin(): Plugin {
  return {
    name: "async-css",
    transformIndexHtml(html) {
      return html.replace(
        /<link\s+rel="stylesheet"\s+([^>]*)\/?>/g,
        (match, attrs) =>
          `<link rel="preload" as="style" ${attrs} onload="this.onload=null;this.rel='stylesheet'"><noscript><link rel="stylesheet" ${attrs}></noscript>`
      );
    },
  };
}
