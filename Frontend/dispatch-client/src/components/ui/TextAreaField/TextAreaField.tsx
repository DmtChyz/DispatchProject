import type { TextareaHTMLAttributes } from "react";

import styles from "./TextAreaField.module.css";

type TextAreaFieldProps = Omit<
  TextareaHTMLAttributes<HTMLTextAreaElement>,
  "onChange" | "value"
> & {
  label: string;
  name: string;
  value: string;
  error?: string;
  helperText?: string;
  onChange: (value: string) => void;
};

export function TextAreaField({
  label,
  name,
  value,
  error,
  helperText,
  className,
  onChange,
  ...props
}: TextAreaFieldProps) {
  const descriptionId = `${name}-description`;
  const textAreaClassName = [
    styles.textarea,
    error ? styles.invalid : "",
    className ?? "",
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <div className={styles.field}>
      <label className={styles.label} htmlFor={name}>
        {label}
      </label>

      <textarea
        {...props}
        id={name}
        name={name}
        value={value}
        className={textAreaClassName}
        aria-invalid={Boolean(error)}
        aria-describedby={
          error || helperText ? descriptionId : undefined
        }
        onChange={(event) => onChange(event.target.value)}
      />

      {(error || helperText) && (
        <p
          id={descriptionId}
          className={error ? styles.error : styles.helper}
        >
          {error ?? helperText}
        </p>
      )}
    </div>
  );
}