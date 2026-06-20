import { useCallback, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import connection from "../clients/hubClient";
import type { HubConnection } from "@microsoft/signalr";
import { useAppSelector } from "@/features/store/hooks";
import type { HubNotification } from "../models/HubNotification";
import { addHangingNotification, clearHangingNotifications, hangingNotifications } from "./useHangingNotifications";
import { showError } from "@/features/shared/utils/toast";
import type { HubActionResponse } from "../models/HubActionResponse";
import { useTranslation } from "react-i18next";
import type { HubActionRequest } from "../models/HubActionRequest";

export type HubMethods = {
  connected: boolean;
  on: HubConnection["on"];
  off: HubConnection["off"];
  invoke: <T = any>(methodName: string, ...args: any[]) => Promise<HubActionResponse<T>>;
  disconnect: () => Promise<void>;
};

export function useHub(): HubMethods {
  const [connected, setConnected] = useState(false);
  const gameState = useAppSelector((state) => state.gameState.gameState);
  const { t } = useTranslation();
  const { i18n } = useTranslation();

  useEffect(() => {
    const start = async () => {
      if (connection.state === signalR.HubConnectionState.Connected) {
        !connected && setConnected(true);
        return;
      }

      if (connection.state === signalR.HubConnectionState.Connecting) {
        return;
      }

      await connection.start();
      setConnected(true);
    };

    connection.onreconnected(() => setConnected(true));
    connection.onclose(() => setConnected(false));

    start();
  }, [connected, setConnected]);

  const disconnect = useCallback(async () => {
    if (connection.state === signalR.HubConnectionState.Connected) {
      await connection.stop();
      setConnected(false);
    }
  }, [setConnected]);

  const on = useCallback((...args: Parameters<HubConnection["on"]>): ReturnType<HubConnection["on"]> => {
    const [methodName, originalHandler] = args;

    const wrappedHandler = (notification: HubNotification<any>) => {
      if (!gameState) {
        addHangingNotification({ handler: originalHandler, notification });
        return;
      }

      return originalHandler(notification);
    };

    return connection.on(methodName, wrappedHandler);
  }, [gameState]);

  const invoke = useCallback(async <T = any>(methodName: string, arg: any) => {
    try {
      const request: HubActionRequest<any> = {
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
  }, []);

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