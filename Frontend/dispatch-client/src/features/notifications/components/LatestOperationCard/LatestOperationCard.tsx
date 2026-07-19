import { Card } from "../../../../components/ui/Card/Card";
import { TRANSLATION_KEYS } from "../../../../constants/translationKeys";
import { useTranslation } from "../../../../hooks/useTranslations";
import type {
  DispatchOperationResponse,
  NotificationOperationStatus,
} from "../../notification.types";
import styles from "./LatestOperationCard.module.css";

type LatestOperationCardProps = {
  operation: DispatchOperationResponse | null;
  responseCode: string | null;
};

const statusTranslationKeys: Record<
  NotificationOperationStatus,
  string
> = {
  queued: TRANSLATION_KEYS.notificationStatus.queued,
  processing: TRANSLATION_KEYS.notificationStatus.processing,
  sent: TRANSLATION_KEYS.notificationStatus.sent,
  failed: TRANSLATION_KEYS.notificationStatus.failed,
};

export function LatestOperationCard({
  operation,
  responseCode,
}: LatestOperationCardProps) {
  const { t } = useTranslation();

  if (operation === null) {
    return (
      <Card
        title={t(
          TRANSLATION_KEYS.dashboard.latestOperationTitle
        )}
        description={t(
          TRANSLATION_KEYS.dashboard.latestOperationDescription
        )}
      >
        <div className={styles.emptyState}>
          <span
            className={styles.emptyIndicator}
            aria-hidden="true"
          />

          <p>
            {t(
              TRANSLATION_KEYS.dashboard.latestOperationEmpty
            )}
          </p>
        </div>
      </Card>
    );
  }

  const statusClassName = {
    queued: styles.queued,
    processing: styles.processing,
    sent: styles.sent,
    failed: styles.failed,
  }[operation.status];

  return (
    <Card
      title={t(
        TRANSLATION_KEYS.dashboard.latestOperationTitle
      )}
      description={t(
        TRANSLATION_KEYS.dashboard.latestOperationDescription
      )}
    >
      <div
        className={styles.content}
        role="status"
        aria-live="polite"
      >
        <div className={styles.statusBox}>
          <span
            className={`${styles.statusIndicator} ${statusClassName}`}
            aria-hidden="true"
          />

          <div>
            <span className={styles.label}>
              {t(
                TRANSLATION_KEYS.dashboard.latestOperationStatus
              )}
            </span>

            <strong className={styles.statusText}>
              {t(statusTranslationKeys[operation.status])}
            </strong>
          </div>
        </div>

        {responseCode && (
          <p className={styles.responseMessage}>
            {t(responseCode)}
          </p>
        )}

        <dl className={styles.details}>
          <div className={styles.detail}>
            <dt>
              {t(
                TRANSLATION_KEYS.notificationForm.correlationId
              )}
            </dt>

            <dd>{operation.correlationId}</dd>
          </div>

          <div className={styles.detail}>
            <dt>
              {t(
                TRANSLATION_KEYS.notificationForm.createdAt
              )}
            </dt>

            <dd>
              {new Date(
                operation.createdAtUtc
              ).toLocaleString()}
            </dd>
          </div>
        </dl>
      </div>
    </Card>
  );
}