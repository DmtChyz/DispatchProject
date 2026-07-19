import type { InputHTMLAttributes } from "react";
import styles from "./InputField.module.css";

type InputFieldProps = Omit<
  InputHTMLAttributes<HTMLInputElement>,
  "onChange" | "value"
> & {
  label: string;
  name: string;
  value: string;
  error?: string;
  helperText?: string;
  onChange: (value: string) => void;
};

export function InputField({
  label,
  name,
  value,
  error,
  helperText,
  onChange,
  type = "text",
  className,
  ...props
}: InputFieldProps) {
  const inputClassName = [
    styles.input,
    error ? styles.inputError : "",
    className ?? "",
  ]
    .filter(Boolean)
    .join(" ");

  const helperId = `${name}-helper`;
  const errorId = `${name}-error`;

  return (
    <label className={styles.field}>
      <span className={styles.label}>{label}</span>

      <input
        {...props}
        id={name}
        name={name}
        type={type}
        value={value}
        className={inputClassName}
        aria-invalid={Boolean(error)}
        aria-describedby={error ? errorId : helperText ? helperId : undefined}
        onChange={(event) => onChange(event.target.value)}
      />

      {error && (
        <span id={errorId} className={styles.error}>
          {error}
        </span>
      )}

      {!error && helperText && (
        <span id={helperId} className={styles.helper}>
          {helperText}
        </span>
      )}
    </label>
  );
}