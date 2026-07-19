import type { ButtonHTMLAttributes, ReactNode } from "react";
import { Spinner } from "../../animated/Spinner/Spinner";
import styles from "./Button.module.css";

type ButtonVariant = "primary" | "secondary" | "ghost" | "danger";
type ButtonSize = "sm" | "md" | "lg";

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  children: ReactNode;
  variant?: ButtonVariant;
  size?: ButtonSize;
  fullWidth?: boolean;
  isLoading?: boolean;
  loadingText?: string;
};

export function Button({
  children,
  variant = "primary",
  size = "md",
  fullWidth = false,
  isLoading = false,
  loadingText,
  disabled,
  className,
  type = "button",
  ...props
}: ButtonProps) {
  const buttonClassName = [
    styles.button,
    styles[variant],
    styles[size],
    fullWidth ? styles.fullWidth : "",
    className ?? "",
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <button
      {...props}
      type={type}
      className={buttonClassName}
      disabled={disabled || isLoading}
    >
      {isLoading && <Spinner size="sm" label="Button loading" />}
      <span>{isLoading && loadingText ? loadingText : children}</span>
    </button>
  );
}