using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Hubs;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;

namespace WebApi.Api.UseCaseSubscribers.Games;

public class StartGameUseCaseSubscriber(
    IApplicationUserResolver _applicationUserResolver,
    HubCallerContextRegistry _hubCallerContextRegistry
) : IUseCaseSubscriber<StartGameUseCase, Empty, Empty>
{
    public async Task ExecuteAsync(UseCaseSubscriberData<Empty, Empty> data, CancellationToken cancellationToken = default)
    {
        var callerContextClients = _hubCallerContextRegistry.Clients;

        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        await callerContextClients
            .Group(applicationUser.GameCode)
            .SendAsync("GameStarted", HubNotification.From(Empty.Value), cancellationToken: cancellationToken);
    }
}
