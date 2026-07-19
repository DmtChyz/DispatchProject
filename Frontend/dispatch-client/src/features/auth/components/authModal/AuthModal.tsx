import { useEffect, useState, type MouseEvent } from "react";
import { createPortal } from "react-dom";

import { Button } from "../../../../components/ui/Button/Button";
import { Card } from "../../../../components/ui/Card/Card";
import { TRANSLATION_KEYS } from "../../../../constants/translationKeys";
import { useTranslation } from "../../../../hooks/useTranslations";
import type { AuthMode } from "../../../../types/auth.types";
import { LoginForm } from "./LoginForm";
import { RegisterForm } from "./RegisterForm";
import styles from "./AuthModal.module.css";

type AuthModalProps = {
  isOpen: boolean;
  initialMode?: AuthMode;
  onClose: () => void;
};

export function AuthModal({
  isOpen,
  initialMode = "login",
  onClose,
}: AuthModalProps) {
  const { t } = useTranslation();
  const [mode, setMode] = useState<AuthMode>(initialMode);

  useEffect(() => {
    if (isOpen) {
      setMode(initialMode);
    }
  }, [isOpen, initialMode]);

  useEffect(() => {
    if (!isOpen) {
      return;
    }

    function handleKeyDown(event: KeyboardEvent) {
      if (event.key === "Escape") {
        onClose();
      }
    }

    const previousOverflow = document.body.style.overflow;

    document.body.style.overflow = "hidden";
    document.addEventListener("keydown", handleKeyDown);

    return () => {
      document.body.style.overflow = previousOverflow;
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [isOpen, onClose]);

  if (!isOpen) {
    return null;
  }

  const isLogin = mode === "login";

  const title = isLogin
    ? t(TRANSLATION_KEYS.auth.loginTitle)
    : t(TRANSLATION_KEYS.auth.registerTitle);

  const description = isLogin
    ? t(TRANSLATION_KEYS.auth.loginDescription)
    : t(TRANSLATION_KEYS.auth.registerDescription);

  function handleOverlayMouseDown(event: MouseEvent<HTMLDivElement>) {
    if (event.target === event.currentTarget) {
      onClose();
    }
  }

  return createPortal(
    <div
      className={styles.overlay}
      onMouseDown={handleOverlayMouseDown}
    >
      <Card
        className={styles.modal}
        role="dialog"
        aria-modal="true"
        aria-label={title}
        title={title}
        description={description}
        actions={
          <Button
            variant="ghost"
            size="sm"
            className={styles.closeButton}
            aria-label={t(TRANSLATION_KEYS.common.close)}
            onClick={onClose}
          >
            ×
          </Button>
        }
      >
        <div
          className={styles.modeSwitch}
          role="tablist"
          aria-label={title}
        >
          <button
            type="button"
            role="tab"
            aria-selected={isLogin}
            className={`${styles.modeButton} ${
              isLogin ? styles.active : ""
            }`}
            onClick={() => setMode("login")}
          >
            {t(TRANSLATION_KEYS.navigation.login)}
          </button>

          <button
            type="button"
            role="tab"
            aria-selected={!isLogin}
            className={`${styles.modeButton} ${
              !isLogin ? styles.active : ""
            }`}
            onClick={() => setMode("register")}
          >
            {t(TRANSLATION_KEYS.navigation.register)}
          </button>
        </div>

        <div role="tabpanel">
          {isLogin ? (
            <LoginForm onSuccess={onClose} />
          ) : (
            <RegisterForm onSuccess={onClose} />
          )}
        </div>
      </Card>
    </div>,
    document.body
  );
}