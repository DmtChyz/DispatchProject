namespace Shared.Constants;

public static class ResponseCodes
{
    public static class Auth
    {
        public const string LoginSuccess = "auth.login.success";
        public const string RegisterSuccess = "auth.register.success";
        public const string LogoutSuccess = "auth.logout.success";

        public const string InvalidCredentials = "auth.invalid_credentials";
        public const string Unauthorized = "auth.unauthorized";
        public const string Forbidden = "auth.forbidden";

        public const string CurrentUserSuccess = "auth.current_user.success";
    }

    public static class Notification
    {
        public const string Queued = "notification.queued";
        public const string Processing = "notification.processing";
        public const string Sent = "notification.sent";
        public const string Failed = "notification.failed";

        public const string QueueFailed = "notification.queue_failed";
    }

    public static class Http
    {
        public const string BadRequest = "http.bad_request";
        public const string NotFound = "http.not_found";
        public const string MethodNotAllowed = "http.method_not_allowed";
        public const string UnsupportedMediaType = "http.unsupported_media_type";
    }

    public static class System
    {
        public const string UnexpectedError = "system.unexpected_error";
    }

    public static class Validation
    {
        public const string InvalidRequest = "validation.invalid_request";
        public static class Fields
        {
            public const string Recipient = "recipient";
        }

        public static class Username
        {
            public const string Required = "validation.username.required";
            public const string Invalid = "validation.username.invalid";
            public const string TooShort = "validation.username.too_short";
            public const string TooLong = "validation.username.too_long";
            public const string AlreadyExists = "validation.username.already_exists";
        }

        public static class Password
        {
            public const string Required = "validation.password.required";
            public const string TooShort = "validation.password.too_short";
            public const string TooLong = "validation.password.too_long";
            public const string Weak = "validation.password.weak";
        }
        public static class Details
        {
            public const string Required = "validation.details.required";
            public const string TooShort = "validation.details.too_short";
            public const string TooLong = "validation.details.too_long";
        }

        public static class ActionType
        {
            public const string Required = "validation.action_type.required";
            public const string Invalid = "validation.action_type.invalid";
        }

        public static class Recipient
        {
            public const string Required = "validation.recipient.required";
            public const string Invalid = "validation.recipient.invalid";
        }

        public const string IdentityFailed = "validation.identity.failed";
    }
}