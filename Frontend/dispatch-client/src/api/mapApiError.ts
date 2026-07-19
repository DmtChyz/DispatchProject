import axios from "axios";

import { RESPONSE_CODES } from "../constants/responseCodes";
import type {
  ApiResponse,
  ValidationFailureData,
} from "../types/api.types";

type TranslateFunction = (key: string) => string;

export type MappedApiError = {
  fields: Record<string, string>;
  generalError?: string;
};

export function mapApiError(
  error: unknown,
  t: TranslateFunction
): MappedApiError {
  if (!axios.isAxiosError<ApiResponse<ValidationFailureData>>(error)) {
    return {
      fields: {},
      generalError: t(RESPONSE_CODES.system.unexpectedError),
    };
  }

  const apiResponse = error.response?.data;

  const responseCode =
    apiResponse?.code ??
    RESPONSE_CODES.system.unexpectedError;

  const fieldCodes =
    apiResponse?.data?.fields ?? {};

  const fields = Object.fromEntries(
    Object.entries(fieldCodes).map(([fieldName, errorCode]) => [
      fieldName,
      t(errorCode),
    ])
  );

  const hasFieldErrors = Object.keys(fields).length > 0;

  return {
    fields,
    generalError: hasFieldErrors
      ? undefined
      : t(responseCode),
  };
}