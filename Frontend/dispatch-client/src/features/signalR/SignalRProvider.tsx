import {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from "react";
import {
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

import { apiClient } from "../../api/apiClient";
import { useAuth } from "../auth/AuthProvider";
import type {
  NotificationStatusUpdate,
  SignalRContextValue,
  SignalRStatus,
} from "./SignalR.types";

const SIGNALR_HUB_PATH = "/notificationHub";
const RECEIVE_NOTIFICATION_EVENT = "ReceiveNotification";
const INITIAL_RETRY_DELAY_MS = 5000;

const SignalRContext =
  createContext<SignalRContextValue | null>(null);

type SignalRProviderProps = {
  children: ReactNode;
};

function getSignalRHubUrl(): string {
  const apiBaseUrl = apiClient.defaults.baseURL;

  if (!apiBaseUrl) {
    throw new Error(
      "Axios baseURL is missing. SignalR hub URL cannot be created."
    );
  }

  const backendBaseUrl = apiBaseUrl.replace(/\/api\/?$/, "");

  return `${backendBaseUrl}${SIGNALR_HUB_PATH}`;
}

export function SignalRProvider({
  children,
}: SignalRProviderProps) {
  const { isAuthenticated, isLoading } = useAuth();

  const [status, setStatus] =
    useState<SignalRStatus>("idle");

  const [latestUpdate, setLatestUpdate] =
    useState<NotificationStatusUpdate | null>(null);

  useEffect(() => {
    if (isLoading) {
      return;
    }

    if (!isAuthenticated) {
      setStatus("idle");
      setLatestUpdate(null);
      return;
    }

    let isDisposed = false;
    let retryTimeout: ReturnType<typeof setTimeout> | null =
      null;

    const connection = new HubConnectionBuilder()
      .withUrl(getSignalRHubUrl(), {
        withCredentials: true,
      })
      .withAutomaticReconnect([
        0,
        2000,
        10000,
        30000,
      ])
      .configureLogging(LogLevel.Warning)
      .build();

    connection.on(
  RECEIVE_NOTIFICATION_EVENT,
  (update: NotificationStatusUpdate) => {
    console.warn(
      "ReceiveNotification received from SignalR:",
      update
    );

    if (isDisposed) {
      return;
    }

    setLatestUpdate(update);
  }
);

    connection.onreconnecting((error) => {
      if (isDisposed) {
        return;
      }

      console.warn(
        "SignalR connection lost. Reconnecting.",
        error
      );

      setStatus("reconnecting");
    });

    connection.onreconnected(() => {
      if (isDisposed) {
        return;
      }

      setStatus("connected");
    });

    connection.onclose((error) => {
      if (isDisposed) {
        return;
      }

      console.error(
        "SignalR connection closed.",
        error
      );

      setStatus("disconnected");

      retryTimeout = setTimeout(() => {
        void startConnection();
      }, INITIAL_RETRY_DELAY_MS);
    });

    async function startConnection() {
      if (isDisposed) {
        return;
      }

      setStatus("connecting");

      try {
        await connection.start();

        if (isDisposed) {
          await connection.stop();
          return;
        }

        setStatus("connected");
      } catch (error) {
        if (isDisposed) {
          return;
        }

        console.error(
          "Failed to establish SignalR connection.",
          error
        );

        setStatus("disconnected");

        retryTimeout = setTimeout(() => {
          void startConnection();
        }, INITIAL_RETRY_DELAY_MS);
      }
    }

    void startConnection();

    return () => {
      isDisposed = true;

      if (retryTimeout !== null) {
        clearTimeout(retryTimeout);
      }

      connection.off(RECEIVE_NOTIFICATION_EVENT);

      void connection.stop();
    };
  }, [isAuthenticated, isLoading]);

  const value = useMemo<SignalRContextValue>(
    () => ({
      status,
      isConnected: status === "connected",
      latestUpdate,
    }),
    [status, latestUpdate]
  );

  return (
    <SignalRContext.Provider value={value}>
      {children}
    </SignalRContext.Provider>
  );
}

export function useSignalR() {
  const context = useContext(SignalRContext);

  if (context === null) {
    throw new Error(
      "useSignalR must be used inside SignalRProvider."
    );
  }

  return context;
}