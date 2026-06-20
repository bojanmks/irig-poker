import { AxiosError, type AxiosRequestConfig, type AxiosResponse } from "axios";
import axiosClient from "../clients/axiosClient";
import type { EndpointResponse } from "../models/EndpointResponse";
import { showError } from "@/features/shared/utils/toast";
import { useTranslation } from "react-i18next";
import type { FieldErrors } from "../models/FieldError";

export function useRequest() {
  const { i18n, t } = useTranslation();

  const send = async<TBody = unknown, TResponse = unknown> (config: AxiosRequestConfig<TBody>): Promise<EndpointResponse<TResponse>> => {
    try {
      config.data ??= config.data = {} as TBody;
      config.headers ??= {};

      config.headers["Accept-Language"] = i18n.language;

      const response = (await axiosClient<EndpointResponse<TResponse>, AxiosResponse<EndpointResponse<TResponse>>, TBody>(config)).data;

      return response;
    }
    catch (error) {
      if (!(error instanceof AxiosError) || !error.status || error.status >= 500) {
        showError(t('common.anErrorOccurred'));
        throw error;
      }

      const errorMessages: string[] = error.response?.data?.errorMessages || [];
      const fieldErrors: FieldErrors[] = error.response?.data?.fieldErrors || [];

      if (!errorMessages.length && !fieldErrors.length) {
        showError(t('common.anErrorOccurred'));
        throw error;
      }

      if (errorMessages.length) {
        errorMessages.forEach(error => {
          showError(error);
        });
      }

      if (fieldErrors.length) {
        Object.values(fieldErrors).forEach(fieldErrors => {
          fieldErrors.errors.forEach(error => showError(error));
        });
      }

      throw error;
    }
  };

  return { send };
}
