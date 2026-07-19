export const RESPONSE_CODES = {
  auth: {
    loginSuccess: "auth.login.success",
    registerSuccess: "auth.register.success",
    logoutSuccess: "auth.logout.success",
    invalidCredentials: "auth.invalid_credentials",
    unauthorized: "auth.unauthorized",
    forbidden: "auth.forbidden",
  },

  notification: {
    queued: "notification.queued",
    processing: "notification.processing",
    sent: "notification.sent",
    failed: "notification.failed",
    queueFailed: "notification.queue_failed",
  },

  http: {
    badRequest: "http.bad_request",
    notFound: "http.not_found",
    methodNotAllowed: "http.method_not_allowed",
    unsupportedMediaType: "http.unsupported_media_type",
  },

  system: {
    unexpectedError: "system.unexpected_error",
  },

  validation: {
    invalidRequest: "validation.invalid_request",

    username: {
      required: "validation.username.required",
      invalid: "validation.username.invalid",
      tooShort: "validation.username.too_short",
      tooLong: "validation.username.too_long",
      alreadyExists: "validation.username.already_exists",
    },

    password: {
      required: "validation.password.required",
      tooShort: "validation.password.too_short",
      tooLong: "validation.password.too_long",
      weak: "validation.password.weak",
    },

    details: {
      required: "validation.details.required",
      tooShort: "validation.details.too_short",
      tooLong: "validation.details.too_long",
    },

    actionType: {
      required: "validation.action_type.required",
      invalid: "validation.action_type.invalid",
    },

    recipient: {
      required: "validation.recipient.required",
      invalid: "validation.recipient.invalid",
    },

    identityFailed: "validation.identity.failed",
  },
} as const;