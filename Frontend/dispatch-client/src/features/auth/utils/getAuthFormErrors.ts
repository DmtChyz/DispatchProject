import axios from "axios";

import { RESPONSE_CODES } from "../../../constants/responseCodes";
import type {
  ApiResponse,
  ValidationFailureData,
} from "../../../types/api.types";

type TranslateFunction = (key: string) => string;

export type AuthFormErrors = {
  userName?: string;
  password?: string;
  form?: string;
};

export function getAuthFormErrors(
  error: unknown,
  t: TranslateFunction
): AuthFormErrors {
  if (!axios.isAxiosError<ApiResponse<ValidationFailureData>>(error)) {
    return {
      form: t(RESPONSE_CODES.system.unexpectedError),
    };
  }

  const response = error.response?.data;
  const fields = response?.data?.fields;

  if (fields) {
    const userNameErrorCode = fields.userName;
    const passwordErrorCode = fields.password;

    return {
      userName: userNameErrorCode
        ? t(userNameErrorCode)
        : undefined,

      password: passwordErrorCode
        ? t(passwordErrorCode)
        : undefined,

      form:
        !userNameErrorCode && !passwordErrorCode
          ? t(response.code)
          : undefined,
    };
  }

  return {
    form: t(
      response?.code ??
        RESPONSE_CODES.system.unexpectedError
    ),
  };
}