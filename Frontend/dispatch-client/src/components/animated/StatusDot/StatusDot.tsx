import styles from "./StatusDot.module.css";

type StatusDotSize = "sm" | "md" | "lg";

type StatusDotProps = {
  isActive: boolean;
  label?: string;
  size?: StatusDotSize;
  pulse?: boolean;
};

export function StatusDot({
  isActive,
  label,
  size = "md",
  pulse = true,
}: StatusDotProps) {
  const dotClassName = [
    styles.dot,
    styles[size],
    isActive ? styles.active : styles.inactive,
    pulse ? styles.pulse : "",
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <span
      className={styles.wrapper}
      role="status"
      aria-label={label ?? (isActive ? "Active" : "Inactive")}
    >
      <span className={dotClassName} aria-hidden="true" />

      {label && <span className={styles.label}>{label}</span>}
    </span>
  );
}