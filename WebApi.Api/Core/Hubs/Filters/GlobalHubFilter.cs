using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Localization;
using WebApi.Common.Core.Localization.Contracts;

namespace WebApi.Api.Core.Hubs.Filters;

internal class GlobalHubFilter(
    ILocaleResolver _localeResolver,
    IApplicationUserResolver _applicationUserResolver
) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next
    )
    {
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
        _applicationUserResolver.SetConnectionId(context.Context.ConnectionId);

        await next(context, exception);
    }
}
