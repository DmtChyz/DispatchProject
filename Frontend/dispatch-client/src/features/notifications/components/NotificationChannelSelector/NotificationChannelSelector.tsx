import type { NotificationActionType } from "../../notification.types";
import styles from "./NotificationChannelSelector.module.css";

type NotificationChannelSelectorProps = {
  value: NotificationActionType;
  label: string;
  emailLabel: string;
  smsLabel: string;
  error?: string;
  onChange: (value: NotificationActionType) => void;
};

export function NotificationChannelSelector({
  value,
  label,
  emailLabel,
  smsLabel,
  error,
  onChange,
}: NotificationChannelSelectorProps) {
  const options: Array<{
    value: NotificationActionType;
    label: string;
    iconClassName: string;
  }> = [
    {
      value: "SendEmail",
      label: emailLabel,
      iconClassName: styles.mailIcon,
    },
    {
      value: "SendSms",
      label: smsLabel,
      iconClassName: styles.letterIcon,
    },
  ];

  return (
    <fieldset
      className={styles.fieldset}
      aria-invalid={Boolean(error)}
    >
      <legend className={styles.legend}>{label}</legend>

      <div className={styles.options}>
        {options.map((option) => {
          const isSelected = value === option.value;

          const optionClassName = [
            styles.option,
            isSelected ? styles.selected : "",
          ]
            .filter(Boolean)
            .join(" ");

          return (
            <label
              key={option.value}
              className={optionClassName}
            >
              <input
                className={styles.input}
                type="radio"
                name="actionType"
                value={option.value}
                checked={isSelected}
                onChange={() => onChange(option.value)}
              />

              <span
                className={`${styles.icon} ${option.iconClassName}`}
                aria-hidden="true"
              />

              <span className={styles.optionLabel}>
                {option.label}
              </span>
            </label>
          );
        })}
      </div>

      {error && <p className={styles.error}>{error}</p>}
    </fieldset>
  );
}