using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using IrigPoker.Application.Core.ApplicationUsers;
using IrigPoker.Application.Core.Localization;
using IrigPoker.Common.Core.Localization.Contracts;

namespace IrigPoker.Api.Core.Hubs.Filters;

internal class GlobalHubFilter(
    ILocaleResolver _localeResolver,
    IHubConnectionIdSetter _hubConnectionIdSetter
) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next
    )
    {
        _hubConnectionIdSetter.SetConnectionId(invocationContext.Context.ConnectionId);

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
        _hubConnectionIdSetter.SetConnectionId(context.Context.ConnectionId);

        await next(context, exception);
    }
}
