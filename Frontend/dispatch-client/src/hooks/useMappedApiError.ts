import { useMemo } from "react";

import {
  mapApiError,
  type MappedApiError,
} from "../api/mapApiError";
import { useTranslation } from "./useTranslations";

export function useMappedApiError(
  error: unknown | null | undefined
): MappedApiError | null {
  const { t } = useTranslation();

  return useMemo(() => {
    if (error === null || error === undefined) {
      return null;
    }

    return mapApiError(error, t);
  }, [error, t]);
}