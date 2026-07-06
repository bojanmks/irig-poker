import type { Language } from "@/features/localization/types/Language";

export type Param<K extends string, V = string> = Partial<Record<K, V>>;

export type LangParams = Param<"lang", Language>;

export type GameCodeParams = Param<"gameCode">;