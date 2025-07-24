using Microsoft.AspNetCore.SignalR;

namespace WebApi.Api.Hubs;

public class GlobalHubFilter(
    HubCallerContextRegistry _hubContextRegistry
) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next
    )
    {
        _hubContextRegistry.SetContext(invocationContext.Context);
        _hubContextRegistry.SetClients(invocationContext.Hub.Clients);

        return await next(invocationContext);
    }

    public async Task OnDisconnectedAsync(
        HubLifetimeContext context,
        Exception? exception,
        Func<HubLifetimeContext, Exception?, Task> next
    )
    {
        _hubContextRegistry.SetContext(context.Context);
        _hubContextRegistry.SetClients(context.Hub.Clients);

        await next(context, exception);
    }
}
