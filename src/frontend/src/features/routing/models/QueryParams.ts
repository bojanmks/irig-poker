export type QueryParams<K extends string = string, V = string> = Record<K, V>;

export type InviteQueryParams = QueryParams<"invite", "true">;
