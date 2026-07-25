import { useCallback, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";

import type { HubConnection } from "@microsoft/signalr";
import * as signalR from "@microsoft/signalr";

import { showError } from "@/features/shared/utils/toast";
import { useAppSelector } from "@/features/store/hooks";

import connection from "../clients/hubClient";
import type { HubActionRequest } from "../models/HubActionRequest";
import type { HubActionResponse } from "../models/HubActionResponse";
import type { HubNotification } from "../models/HubNotification";
import { addHangingNotification, clearHangingNotifications, hangingNotifications } from "../util/hangingNotifications";

export type HubMethods = {
  connected: boolean;
  on: HubConnection["on"];
  off: HubConnection["off"];
  invoke: <T = unknown>(methodName: string, arg: unknown) => Promise<HubActionResponse<T>>;
  disconnect: () => Promise<void>;
};

type UseHubParams = {
  onDisconnected?: () => void;
} | undefined;

export function useHub(params?: UseHubParams): HubMethods {
  const { onDisconnected } = params || {};

  const [connected, setConnected] = useState(false);
  const gameState = useAppSelector((state) => state.gameState.gameState);
  const { t } = useTranslation();
  const { i18n } = useTranslation();

  useEffect(() => {
    const start = async () => {
      if (connection.state === signalR.HubConnectionState.Connected) {
        setConnected(true);
        return;
      }

      if (connection.state !== signalR.HubConnectionState.Disconnected) {
        return;
      }

      await connection.start();
      setConnected(true);
    };

    start();
  }, [setConnected]);

  useEffect(() => {
    const handler = () => {
      setConnected(false);
      onDisconnected?.();
    };

    connection.onclose(handler);

    return () => {
      connection['_closedCallbacks'] = connection['_closedCallbacks'].filter((x: unknown) => x !== handler);
    };
  }, [setConnected, onDisconnected]);

  const disconnect = useCallback(async () => {
    if (connection.state === signalR.HubConnectionState.Connected) {
      await connection.stop();
      setConnected(false);
    }
  }, [setConnected]);

  const on = useCallback((...args: Parameters<HubConnection["on"]>): ReturnType<HubConnection["on"]> => {
    const [methodName, originalHandler] = args;

    const wrappedHandler = (notification: HubNotification<unknown>) => {
      if (!gameState) {
        addHangingNotification({ handler: originalHandler, notification });
        return;
      }

      return originalHandler(notification);
    };

    return connection.on(methodName, wrappedHandler);
  }, [gameState]);

  const invoke = useCallback(async <T = unknown>(methodName: string, arg: unknown) => {
    try {
      const request: HubActionRequest<unknown> = {
        data: arg,
        languageCode: i18n.language
      };

      const response: HubActionResponse<T> = await connection.invoke(methodName, request);
      
      if (response.isSuccess) {
        return response;
      }
      
      if (response.errors && response.errors.length > 0) {
        response.errors.forEach(error => {
          showError(error);
        });
      }

      if (response.fieldErrors) {
        Object.values(response.fieldErrors).forEach(fieldErrors => {
          fieldErrors.errors.forEach(error => showError(error));
        });
      }

      return response;
    }
    catch (error) {
      showError(t('common.anErrorOccurred'));
      throw error;
    }
  }, [i18n, t]);

  useEffect(() => {
    if (!gameState || !hangingNotifications.length) {
      return;
    }

    const sortedItems = hangingNotifications.sort((a, b) =>
      new Date(a.notification.timestamp).getTime() - new Date(b.notification.timestamp).getTime()
    );

    sortedItems.forEach(async ({ handler, notification }) => {
      await handler(notification);
    });

    clearHangingNotifications();
  }, [gameState]);

  return {
    connected,
    on,
    off: connection.off.bind(connection),
    invoke,
    disconnect
  };
}