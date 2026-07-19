namespace DispatchMicroservice.Hubs;

public static class NotificationHubGroups
{
    public static string User(string userId)
    {
        return $"user:{userId}";
    }
}