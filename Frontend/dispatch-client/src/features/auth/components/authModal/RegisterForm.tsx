import { useState, type FormEvent } from "react";

import { Button } from "../../../../components/ui/Button/Button";
import { InputField } from "../../../../components/ui/InputField/InputField";
import { TRANSLATION_KEYS } from "../../../../constants/translationKeys";
import { useMappedApiError } from "../../../../hooks/useMappedApiError";
import { useTranslation } from "../../../../hooks/useTranslations";
import { useAuth } from "../../AuthProvider";
import type { AuthCredentials } from "../../../../types/auth.types";
import styles from "./AuthForm.module.css";

type RegisterFormProps = {
  onSuccess: () => void;
};

const initialCredentials: AuthCredentials = {
  userName: "",
  password: "",
};

export function RegisterForm({ onSuccess }: RegisterFormProps) {
  const { register } = useAuth();
  const { t } = useTranslation();

  const [credentials, setCredentials] =
    useState<AuthCredentials>(initialCredentials);

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [requestError, setRequestError] = useState<unknown>(null);

  const apiError = useMappedApiError(requestError);

  function updateField(
    field: keyof AuthCredentials,
    value: string
  ) {
    setRequestError(null);

    setCredentials((current) => ({
      ...current,
      [field]: value,
    }));
  }

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>
  ) {
    event.preventDefault();

    setIsSubmitting(true);
    setRequestError(null);

    try {
      await register({
        userName: credentials.userName.trim(),
        password: credentials.password,
      });

      onSuccess();
    } catch (error) {
      setRequestError(error);
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <form
      className={styles.form}
      onSubmit={handleSubmit}
      noValidate
    >
      <div className={styles.fields}>
        <InputField
          label={t(TRANSLATION_KEYS.auth.usernameLabel)}
          name="register-userName"
          value={credentials.userName}
          error={apiError?.fields.userName}
          placeholder={t(
            TRANSLATION_KEYS.auth.usernamePlaceholder
          )}
          autoComplete="username"
          autoFocus
          disabled={isSubmitting}
          onChange={(value) =>
            updateField("userName", value)
          }
        />

        <InputField
          label={t(TRANSLATION_KEYS.auth.passwordLabel)}
          name="register-password"
          type="password"
          value={credentials.password}
          error={apiError?.fields.password}
          placeholder={t(
            TRANSLATION_KEYS.auth.passwordPlaceholder
          )}
          autoComplete="new-password"
          disabled={isSubmitting}
          onChange={(value) =>
            updateField("password", value)
          }
        />
      </div>

      {apiError?.generalError && (
        <p
          className={styles.generalError}
          role="alert"
          aria-live="polite"
        >
          {apiError.generalError}
        </p>
      )}

      <Button
        type="submit"
        fullWidth
        isLoading={isSubmitting}
        loadingText={t(
          TRANSLATION_KEYS.auth.registerLoading
        )}
      >
        {t(TRANSLATION_KEYS.auth.registerButton)}
      </Button>
    </form>
  );
}