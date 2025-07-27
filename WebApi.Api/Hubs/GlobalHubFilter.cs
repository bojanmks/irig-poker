using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Localization;
using WebApi.Common.DTO;

namespace WebApi.Api.Hubs;

public class GlobalHubFilter(
    HubCallerContextRegistry _hubContextRegistry,
    ILocaleResolver _localeResolver,
    IApplicationUserResolver _applicationUserResolver
) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next
    )
    {
        _hubContextRegistry.SetContext(invocationContext.Context);
        _hubContextRegistry.SetClients(invocationContext.Hub.Clients);

        _applicationUserResolver.SetConnectionId(invocationContext.Context.ConnectionId);

        foreach (var argument in invocationContext.HubMethodArguments)
        {
            if (argument is IHasLocaleInfo argWithLocaleInfo && !string.IsNullOrWhiteSpace(argWithLocaleInfo.LanguageCode))
            {
                _localeResolver.ForceLocale(new CultureInfo(argWithLocaleInfo.LanguageCode));
            }
        }

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

        _applicationUserResolver.SetConnectionId(context.Context.ConnectionId);

        await next(context, exception);
    }
}
