import { useState, type FormEvent } from "react";

import { Button } from "../../../../components/ui/Button/Button";
import { Card } from "../../../../components/ui/Card/Card";
import { InputField } from "../../../../components/ui/InputField/InputField";
import { TextAreaField } from "../../../../components/ui/TextAreaField/TextAreaField";
import { TRANSLATION_KEYS } from "../../../../constants/translationKeys";
import { useMappedApiError } from "../../../../hooks/useMappedApiError";
import { useTranslation } from "../../../../hooks/useTranslations";
import { sendNotification } from "../../api/api";
import type {
  DispatchOperationResponse,
  NotificationActionType,
} from "../../notification.types";
import { NotificationChannelSelector } from "../NotificationChannelSelector/NotificationChannelSelector";
import styles from "./NotificationForm.module.css";

type NotificationFormProps = {
  onAccepted: (
    operation: DispatchOperationResponse,
    responseCode: string
  ) => void;
};

export function NotificationForm({
  onAccepted,
}: NotificationFormProps) {
  const { t } = useTranslation();

  const [actionType, setActionType] =
    useState<NotificationActionType>("SendEmail");
  const [recipient, setRecipient] = useState("");
  const [details, setDetails] = useState("");

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [requestError, setRequestError] =
    useState<unknown | null>(null);

  const mappedError = useMappedApiError(requestError);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    setIsSubmitting(true);
    setRequestError(null);

    try {
      const response = await sendNotification({
        actionType,
        recipient: recipient.trim(),
        details: details.trim(),
      });

      if (!response.data) {
        throw new Error(
          "The dispatch response does not contain operation data."
        );
      }

      onAccepted(response.data, response.code);

      setRecipient("");
      setDetails("");
    } catch (error) {
      setRequestError(error);
    } finally {
      setIsSubmitting(false);
    }
  }

  function handleActionTypeChange(value: NotificationActionType) {
    setActionType(value);
    setRequestError(null);
  }

  return (
    <Card
      title={t(TRANSLATION_KEYS.notificationForm.title)}
      description={t(
        TRANSLATION_KEYS.notificationForm.description
      )}
    >
      <form className={styles.form} onSubmit={handleSubmit}>
        <NotificationChannelSelector
          value={actionType}
          label={t(
            TRANSLATION_KEYS.notificationForm.channelLabel
          )}
          emailLabel={t(
            TRANSLATION_KEYS.notificationForm.email
          )}
          smsLabel={t(
            TRANSLATION_KEYS.notificationForm.sms
          )}
          error={mappedError?.fields.actionType}
          onChange={handleActionTypeChange}
        />

        <InputField
          label={t(
            TRANSLATION_KEYS.notificationForm.recipientLabel
          )}
          name="recipient"
          type={actionType === "SendEmail" ? "email" : "tel"}
          value={recipient}
          placeholder={t(
            TRANSLATION_KEYS.notificationForm
              .recipientPlaceholder
          )}
          error={mappedError?.fields.recipient}
          onChange={(value) => {
            setRecipient(value);
            setRequestError(null);
          }}
        />

        <TextAreaField
          label={t(
            TRANSLATION_KEYS.notificationForm.detailsLabel
          )}
          name="details"
          value={details}
          minLength={20}
          maxLength={256}
          placeholder={t(
            TRANSLATION_KEYS.notificationForm
              .detailsPlaceholder
          )}
          helperText={`${details.length}/256`}
          error={mappedError?.fields.details}
          onChange={(value) => {
            setDetails(value);
            setRequestError(null);
          }}
        />

        {mappedError?.generalError && (
          <p className={styles.generalError} role="alert">
            {mappedError.generalError}
          </p>
        )}

        <div className={styles.actions}>
          <Button
            type="submit"
            size="lg"
            fullWidth
            isLoading={isSubmitting}
            loadingText={t(
              TRANSLATION_KEYS.notificationForm.submitting
            )}
          >
            {t(TRANSLATION_KEYS.notificationForm.submit)}
          </Button>
        </div>
      </form>
    </Card>
  );
}