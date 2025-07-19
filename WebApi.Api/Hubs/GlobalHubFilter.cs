using Microsoft.AspNetCore.SignalR;

namespace WebApi.Api.Hubs;

public class GlobalHubFilter(
    HubContextRegistry _hubContextRegistry
) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        _hubContextRegistry.SetContext(invocationContext.Context);
        _hubContextRegistry.SetClients(invocationContext.Hub.Clients);

        return await next(invocationContext);
    }
}
