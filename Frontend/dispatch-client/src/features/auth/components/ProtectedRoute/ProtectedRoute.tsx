import type { ReactNode } from "react";
import { Navigate, useLocation } from "react-router-dom";

import { TRANSLATION_KEYS } from "../../../../constants/translationKeys";
import { useTranslation } from "../../../../hooks/useTranslations";
import { useAuth } from "../../AuthProvider";
import styles from "./ProtectedRoute.module.css";

type ProtectedRouteProps = {
  children: ReactNode;
};

export function ProtectedRoute({ children }: ProtectedRouteProps) {
  const { isAuthenticated, isLoading } = useAuth();
  const { t } = useTranslation();
  const location = useLocation();

  if (isLoading) {
    return (
      <div className={styles.loader} role="status" aria-live="polite">
        {t(TRANSLATION_KEYS.common.loading)}
      </div>
    );
  }

  if (!isAuthenticated) {
    return (
      <Navigate
        to="/"
        replace
        state={{ from: location.pathname }}
      />
    );
  }

  return <>{children}</>;
}