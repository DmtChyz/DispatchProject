import { en } from "./en";
import { uk } from "./uk";

export type Language = "en" | "uk";

type TranslationDictionary = Readonly<Record<string, string>>;

export const translations: Record<Language, TranslationDictionary> = {
  en,
  uk,
};