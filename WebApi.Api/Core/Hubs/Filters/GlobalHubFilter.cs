using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using WebApi.Api.Core.Hubs.Registries;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Localization;
using WebApi.Common.Core.Localization.Contracts;

namespace WebApi.Api.Core.Hubs.Filters;

internal class GlobalHubFilter(
    IHubCallerContextSetter _hubCallerContextSetter,
    ILocaleResolver _localeResolver,
    IApplicationUserResolver _applicationUserResolver
) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next
    )
    {
        _hubCallerContextSetter.SetContext(invocationContext.Context);
        _hubCallerContextSetter.SetClients(invocationContext.Hub.Clients);

        _applicationUserResolver.SetConnectionId(invocationContext.Context.ConnectionId);

        foreach (var argument in invocationContext.HubMethodArguments)
        {
            if (argument is IHasLocaleInfo argWithLocaleInfo && !string.IsNullOrWhiteSpace(argWithLocaleInfo.LanguageCode))
            {
                _localeResolver.ForceLocale(new CultureInfo(argWithLocaleInfo.LanguageCode));
            }
        }

        try
        {
            return await next(invocationContext);
        }
        finally
        {
            _hubCallerContextSetter.Clear();
        }
    }

    public async Task OnDisconnectedAsync(
        HubLifetimeContext context,
        Exception? exception,
        Func<HubLifetimeContext, Exception?, Task> next
    )
    {
        _hubCallerContextSetter.SetContext(context.Context);
        _hubCallerContextSetter.SetClients(context.Hub.Clients);

        _applicationUserResolver.SetConnectionId(context.Context.ConnectionId);

        try
        {
            await next(context, exception);
        }
        finally
        {
            _hubCallerContextSetter.Clear();
        }
    }
}
