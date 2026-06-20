/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_MIN_PLAYERS_TO_START: number;
  readonly VITE_API_BASE_URL: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}