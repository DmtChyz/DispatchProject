import { useCallback } from "react";
import { useSettings } from "../features/settings/SettingsProvider";
import { translations } from "../localization/translation";

export function useTranslation() {
  const { language } = useSettings();

  const t = useCallback(
    (key: string): string =>
      translations[language][key] ??
      translations.en[key] ??
      key,
    [language]
  );

  return { t };
}