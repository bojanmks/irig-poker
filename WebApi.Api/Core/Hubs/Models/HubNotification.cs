namespace WebApi.Api.Core.Hubs.Models;

public class HubNotification<T>
{
    public required T Data { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
}

public static class HubNotification
{
    public static HubNotification<T> From<T>(T data, TimeProvider timeProvider)
    {
        return new HubNotification<T>
        {
            Data = data,
            Timestamp = timeProvider.GetUtcNow()
        };
    }
}