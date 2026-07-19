import type { ReactNode } from "react";
import styles from "./Badge.module.css";

type BadgeVariant =
  | "default"
  | "success"
  | "danger"
  | "warning"
  | "info"
  | "muted";

type BadgeProps = {
  children: ReactNode;
  variant?: BadgeVariant;
  isLoading?: boolean;
};

export function Badge({
  children,
  variant = "default",
  isLoading = false,
}: BadgeProps) {
  return (
    <span
      className={`${styles.badge} ${styles[variant]} ${
        isLoading ? styles.loading : ""
      }`}
      aria-busy={isLoading}
    >
      {isLoading && <span className={styles.spinner} aria-hidden="true" />}

      <span>{children}</span>
    </span>
  );
}