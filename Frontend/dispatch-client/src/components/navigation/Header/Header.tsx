import { useState } from "react";
import { NavLink } from "react-router-dom";

import { StatusDot } from "../../animated/StatusDot/StatusDot";
import { Button } from "../../ui/Button/Button";
import { TRANSLATION_KEYS } from "../../../constants/translationKeys";
import { useAuth } from "../../../features/auth/AuthProvider";
import { AuthModal } from "../../../features/auth/components/authModal/AuthModal";
import { LanguageSwitch } from "../../../features/settings/LanguageSwitch/LanguageSwitch";
import { ThemeSwitch } from "../../../features/settings/ThemeSwitch/ThemeSwitch";
import { useTranslation } from "../../../hooks/useTranslations";
import type { AuthMode } from "../../../types/auth.types";
import styles from "./Header.module.css";

export function Header() {
  const { t } = useTranslation();
  const { user, isAuthenticated, isLoading, logout } = useAuth();

  const [authMode, setAuthMode] = useState<AuthMode | null>(null);
  const [isLoggingOut, setIsLoggingOut] = useState(false);

  async function handleLogout() {
    setIsLoggingOut(true);

    try {
      await logout();
    } catch (error) {
      console.error("Failed to log out.", error);
    } finally {
      setIsLoggingOut(false);
    }
  }

  return (
    <>
      <header className={styles.header}>
        <div className={styles.container}>
          <NavLink to="/" className={styles.logo}>
            Dispatch
          </NavLink>

          <nav className={styles.nav}>
            <NavLink
              to="/dashboard"
              className={({ isActive }) =>
                `${styles.navLink} ${isActive ? styles.active : ""}`
              }
            >
              {t(TRANSLATION_KEYS.navigation.dashboard)}
            </NavLink>
          </nav>

          <div className={styles.actions}>
            {isLoading ? (
              <Button
                size="sm"
                variant="ghost"
                isLoading
                loadingText={t(TRANSLATION_KEYS.common.loading)}
                disabled
              >
                {t(TRANSLATION_KEYS.navigation.login)}
              </Button>
            ) : isAuthenticated && user ? (
              <div className={styles.userAccount}>
                <StatusDot
                  isActive
                  size="sm"
                  pulse={false}
                  label={undefined}
                />

                <span className={styles.userName}>
                  {user.userName}
                </span>

                <Button
                  type="button"
                  variant="ghost"
                  size="sm"
                  className={styles.logoutButton}
                  disabled={isLoggingOut}
                  aria-label={t(TRANSLATION_KEYS.navigation.logout)}
                  title={t(TRANSLATION_KEYS.navigation.logout)}
                  onClick={handleLogout}
                >
                  <svg
                    className={styles.logoutIcon}
                    viewBox="0 0 24 24"
                    fill="none"
                    aria-hidden="true"
                  >
                    <path
                      d="M6 6L18 18M18 6L6 18"
                      stroke="currentColor"
                      strokeWidth="2.2"
                      strokeLinecap="round"
                    />
                  </svg>
                </Button>
              </div>
            ) : (
              <Button
                size="sm"
                onClick={() => setAuthMode("login")}
              >
                {t(TRANSLATION_KEYS.navigation.login)}
              </Button>
            )}

            <LanguageSwitch />
            <ThemeSwitch />
          </div>
        </div>
      </header>

      <AuthModal
        isOpen={authMode !== null}
        initialMode={authMode ?? "login"}
        onClose={() => setAuthMode(null)}
      />
    </>
  );
}