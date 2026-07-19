import { useState, type FormEvent } from "react";

import { Button } from "../../../../components/ui/Button/Button";
import { InputField } from "../../../../components/ui/InputField/InputField";
import { TRANSLATION_KEYS } from "../../../../constants/translationKeys";
import { useMappedApiError } from "../../../../hooks/useMappedApiError";
import { useTranslation } from "../../../../hooks/useTranslations";
import type { AuthCredentials } from "../../../../types/auth.types";
import { useAuth } from "../../AuthProvider";
import styles from "./AuthForm.module.css";

type LoginFormProps = {
  onSuccess: () => void;
};

const initialCredentials: AuthCredentials = {
  userName: "",
  password: "",
};

export function LoginForm({ onSuccess }: LoginFormProps) {
  const { login } = useAuth();
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
      await login({
        userName: credentials.userName.trim(),
        password: credentials.password,
      });

      setCredentials(initialCredentials);
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
          name="login-userName"
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
          name="login-password"
          type="password"
          value={credentials.password}
          error={apiError?.fields.password}
          placeholder={t(
            TRANSLATION_KEYS.auth.passwordPlaceholder
          )}
          autoComplete="current-password"
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
          TRANSLATION_KEYS.auth.loginLoading
        )}
      >
        {t(TRANSLATION_KEYS.auth.loginButton)}
      </Button>
    </form>
  );
}