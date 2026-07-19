public static class ValidationPatterns
{
    public const string Phone = @"^\+[1-9](?!0{6,14}$)[0-9]{6,14}$";
    public const string Email = @"^([^\.]([\w\.\-]+))@([\w\-]+)((\.(\w){1,})+)$";
}