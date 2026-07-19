import {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from "react";

type Theme = "dark" | "light";
type Language = "en" | "uk";

type UserSettings = {
  theme: Theme;
  language: Language;
};

type SettingsContextValue = {
  settings: UserSettings;
  theme: Theme;
  language: Language;
  setTheme: (theme: Theme) => void;
  toggleTheme: () => void;
  setLanguage: (language: Language) => void;
};

const SETTINGS_STORAGE_KEY = "dispatch_user_settings";

const defaultSettings: UserSettings = {
  theme: "dark",
  language: "en",
};

const SettingsContext = createContext<SettingsContextValue | null>(null);

type SettingsProviderProps = {
  children: ReactNode;
};

function isTheme(value: unknown): value is Theme {
  return value === "dark" || value === "light";
}

function isLanguage(value: unknown): value is Language {
  return value === "en" || value === "ua";
}

function readSettingsFromStorage(): UserSettings {
  const rawSettings = localStorage.getItem(SETTINGS_STORAGE_KEY);

  if (!rawSettings) {
    return defaultSettings;
  }

  try {
    const parsedSettings = JSON.parse(rawSettings) as Partial<UserSettings>;

    return {
      theme: isTheme(parsedSettings.theme)
        ? parsedSettings.theme
        : defaultSettings.theme,

      language: isLanguage(parsedSettings.language)
        ? parsedSettings.language
        : defaultSettings.language,
    };
  } catch {
    localStorage.removeItem(SETTINGS_STORAGE_KEY);
    return defaultSettings;
  }
}

export function SettingsProvider({ children }: SettingsProviderProps) {
  const [settings, setSettings] = useState<UserSettings>(() =>
    readSettingsFromStorage()
  );

  useEffect(() => {
    localStorage.setItem(SETTINGS_STORAGE_KEY, JSON.stringify(settings));
    document.documentElement.dataset.theme = settings.theme;
    document.documentElement.lang =
      document.documentElement.lang = settings.language;
  }, [settings]);

  function setTheme(theme: Theme) {
    setSettings((previousSettings) => ({
      ...previousSettings,
      theme,
    }));
  }

  function toggleTheme() {
    setSettings((previousSettings) => ({
      ...previousSettings,
      theme: previousSettings.theme === "dark" ? "light" : "dark",
    }));
  }

  function setLanguage(language: Language) {
    setSettings((previousSettings) => ({
      ...previousSettings,
      language,
    }));
  }

  const value = useMemo<SettingsContextValue>(
    () => ({
      settings,
      theme: settings.theme,
      language: settings.language,
      setTheme,
      toggleTheme,
      setLanguage,
    }),
    [settings]
  );

  return (
    <SettingsContext.Provider value={value}>
      {children}
    </SettingsContext.Provider>
  );
}

export function useSettings() {
  const context = useContext(SettingsContext);

  if (context === null) {
    throw new Error("useSettings must be used inside SettingsProvider.");
  }

  return context;
}