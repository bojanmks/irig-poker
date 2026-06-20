namespace WebApi.Api.Core.Hubs.Models;

public class HubNotification<T>
{
    public required T Data { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

public static class HubNotification
{
    public static HubNotification<T> From<T>(T data)
    {
        return new HubNotification<T>
        {
            Data = data
        };
    }
}