using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Models;
using WebApi.Api.Core.Hubs.Registries;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.UseCases;

namespace WebApi.Api.Features.Games.UseCaseSubscribers;

public class StartGameUseCaseSubscriber(
    IApplicationUserResolver _applicationUserResolver,
    IHubCallerContextAccessor _hubCallerContextAccessor
) : IUseCaseSubscriber<StartGameUseCase, Empty, Empty>
{
    public async Task ExecuteAsync(UseCaseSubscriberData<Empty, Empty> data, CancellationToken cancellationToken = default)
    {
        var callerContextClients = _hubCallerContextAccessor.Clients;

        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        await callerContextClients
            .Group(applicationUser.GameCode)
            .SendAsync("GameStarted", HubNotification.From(Empty.Value), cancellationToken: cancellationToken);
    }
}
