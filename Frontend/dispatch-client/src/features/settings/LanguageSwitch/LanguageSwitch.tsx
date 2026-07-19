import { useSettings } from "../../../features/settings/SettingsProvider";
import styles from "./LanguageSwitch.module.css";

const FLAG_PATHS = {
  en: "/flags/BR.svg",
  uk: "/flags/UK.svg",
} as const;

export function LanguageSwitch() {
  const { language, setLanguage } = useSettings();

  return (
    <div
      className={styles.languageSwitch}
      role="group"
      aria-label="Select interface language"
    >
      <button
        type="button"
        className={`${styles.languageOption} ${
          language === "en" ? styles.active : ""
        }`}
        aria-label="Switch to English"
        aria-pressed={language === "en"}
        onClick={() => setLanguage("en")}
      >
        <img
          src={FLAG_PATHS.en}
          alt=""
          className={styles.flag}
          aria-hidden="true"
        />

        <span>EN</span>
      </button>

      <button
        type="button"
        className={`${styles.languageOption} ${
          language === "uk" ? styles.active : ""
        }`}
        aria-label="Перейти на українську"
        aria-pressed={language === "uk"}
        onClick={() => setLanguage("uk")}
      >
        <img
          src={FLAG_PATHS.uk}
          alt=""
          className={styles.flag}
          aria-hidden="true"
        />

        <span>УКР</span>
      </button>
    </div>
  );
}