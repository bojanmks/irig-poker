import type { QueryParam } from "../consts/QueryParam";

export type QueryParams<K extends string = string, V = string> = Record<K, V>;

export type AutoLangQueryParams = QueryParams<(typeof QueryParam)["AutoLang"], "">;
