namespace Shared.Constants
{
    public static class AuthCookieConstants
    {
        public const string Name = "access_token";
        public const string Path = "/";
        public static readonly TimeSpan Lifetime = TimeSpan.FromHours(1);
    }
}
