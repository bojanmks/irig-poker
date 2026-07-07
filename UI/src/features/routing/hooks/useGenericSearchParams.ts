import { type NavigateOptions,useSearchParams } from "react-router-dom";

export function useGenericSearchParams<T extends Record<string, string>>() {
  const [searchParams, setSearchParams] = useSearchParams();

  const params = Object.fromEntries(searchParams.entries()) as Partial<T>;

  const setParams = (
    newParams: Partial<T> | ((prev: Partial<T>) => Partial<T>),
    options?: NavigateOptions
  ) => {
    const resolvedParams = typeof newParams === 'function' ? newParams(params) : newParams;
    
    // Filter out undefined or null values to keep the URL clean
    const cleanParams = Object.entries(resolvedParams).reduce((acc, [key, value]) => {
      if (value !== undefined && value !== null) {
        acc[key] = String(value);
      }
      return acc;
    }, {} as Record<string, string>);

    setSearchParams(cleanParams, options);
  };

  return [params, setParams] as const;
}