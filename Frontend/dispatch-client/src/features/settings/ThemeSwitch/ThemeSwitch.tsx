import moonIcon from "../../../../public/icons/moon.svg";
import sunIcon from "../../../../public/icons/sun.svg";

import { useSettings } from "../SettingsProvider";
import styles from "./ThemeSwitch.module.css";

export function ThemeSwitch() {
  const { theme, toggleTheme } = useSettings();

  const isDarkTheme = theme === "dark";

  return (
    <button
      type="button"
      className={styles.switch}
      onClick={toggleTheme}
      aria-label={isDarkTheme ? "Switch to light theme" : "Switch to dark theme"}
      title={isDarkTheme ? "Switch to light theme" : "Switch to dark theme"}
    >
      <img
        src={isDarkTheme ? moonIcon : sunIcon}
        alt=""
        aria-hidden="true"
        className={`${styles.icon} ${
          isDarkTheme ? styles.darkIcon : styles.lightIcon
        }`}
      />
    </button>
  );
}