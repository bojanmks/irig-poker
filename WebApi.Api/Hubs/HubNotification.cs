namespace WebApi.Api.Hubs;

public class HubNotification<T>
{
    public T Data { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
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