import { useEffect, useState } from "react";

import { StatusDot } from "../../components/animated/StatusDot/StatusDot";
import { PageHeader } from "../../components/ui/PageHeader/PageHeader";
import { TRANSLATION_KEYS } from "../../constants/translationKeys";
import { LatestOperationCard } from "../../features/notifications/components/LatestOperationCard/LatestOperationCard";
import { NotificationForm } from "../../features/notifications/components/NotificationForm/NotificationForm";
import type {
  DispatchOperationResponse,
  NotificationOperationStatus,
} from "../../features/notifications/notification.types";
import {
  SIGNALR_ACTION_STATUS,
  type SignalRActionStatus,
} from "../../features/signalR/SignalR.types";
import { useSignalR } from "../../features/signalR/SignalRProvider";
import { useTranslation } from "../../hooks/useTranslations";
import styles from "./DashboardPage.module.css";

type LatestOperationState = {
  operation: DispatchOperationResponse | null;
  responseCode: string | null;
};

function mapSignalRStatus(
  status: SignalRActionStatus
): NotificationOperationStatus {
  return status === SIGNALR_ACTION_STATUS.completed
    ? "sent"
    : "failed";
}

function getResponseCode(
  status: NotificationOperationStatus
): string {
  return status === "sent"
    ? "notification.sent"
    : "notification.failed";
}

export function DashboardPage() {
  const { t } = useTranslation();
  const { status, isConnected, latestUpdate } = useSignalR();

  const [latestResult, setLatestResult] =
    useState<LatestOperationState>({
      operation: null,
      responseCode: null,
    });

  const currentCorrelationId =
    latestResult.operation?.correlationId ?? null;

  useEffect(() => {
    if (
      latestUpdate === null ||
      currentCorrelationId === null ||
      latestUpdate.correlationId !== currentCorrelationId
    ) {
      return;
    }

    const updatedStatus = mapSignalRStatus(
      latestUpdate.status
    );

    setLatestResult((current) => {
      if (
        current.operation === null ||
        current.operation.correlationId !==
          latestUpdate.correlationId
      ) {
        return current;
      }

      return {
        operation: {
          ...current.operation,
          status: updatedStatus,
        },
        responseCode: getResponseCode(updatedStatus),
      };
    });
  }, [latestUpdate, currentCorrelationId]);

  const statusTranslationKey = {
    idle: TRANSLATION_KEYS.signalR.statusDisconnected,
    connecting: TRANSLATION_KEYS.signalR.statusConnecting,
    connected: TRANSLATION_KEYS.signalR.statusConnected,
    reconnecting:
      TRANSLATION_KEYS.signalR.statusReconnecting,
    disconnected:
      TRANSLATION_KEYS.signalR.statusDisconnected,
  }[status];

  function handleNotificationAccepted(
    operation: DispatchOperationResponse,
    responseCode: string
  ) {
    if (
      latestUpdate !== null &&
      latestUpdate.correlationId === operation.correlationId
    ) {
      const updatedStatus = mapSignalRStatus(
        latestUpdate.status
      );

      setLatestResult({
        operation: {
          ...operation,
          status: updatedStatus,
        },
        responseCode: getResponseCode(updatedStatus),
      });

      return;
    }

    setLatestResult({
      operation,
      responseCode,
    });
  }

  return (
    <div className={styles.page}>
      <PageHeader
        eyebrow={t(TRANSLATION_KEYS.dashboard.eyebrow)}
        title={t(TRANSLATION_KEYS.dashboard.title)}
        description={t(
          TRANSLATION_KEYS.dashboard.description
        )}
        actions={
          <div className={styles.connectionStatus}>
            <StatusDot
              isActive={isConnected}
              label={t(statusTranslationKey)}
              pulse={
                status === "connecting" ||
                status === "reconnecting"
              }
            />
          </div>
        }
      />

      <section className={styles.workspace}>
        <div className={styles.formColumn}>
          <NotificationForm
            onAccepted={handleNotificationAccepted}
          />
        </div>

        <aside className={styles.operationColumn}>
          <LatestOperationCard
            operation={latestResult.operation}
            responseCode={latestResult.responseCode}
          />
        </aside>
      </section>
    </div>
  );
}