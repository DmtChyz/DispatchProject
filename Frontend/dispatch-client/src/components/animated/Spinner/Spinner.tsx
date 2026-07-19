import styles from "./Spinner.module.css";

type SpinnerSize = "sm" | "md" | "lg";

type SpinnerProps = {
  size?: SpinnerSize;
  label?: string;
};

export function Spinner({ size = "md", label = "Loading" }: SpinnerProps) {
  return (
    <span className={styles.wrapper} role="status" aria-label={label}>
      <span className={`${styles.spinner} ${styles[size]}`} />
    </span>
  );
}