using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Models;
using WebApi.Api.Core.Hubs.Registries;
using WebApi.Api.Features.Games.Hubs;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Common.Features.Games.Joining;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Api.Features.Games.UseCaseSubscribers;

public class JoinGameUseCaseSubscriber(
    IHubContext<GameHub> _hubContext,
    HubCallerContextRegistry _hubCallerContextRegistry
) : IUseCaseSubscriber<JoinGameUseCase, JoinGameDto, PublicGameStateDto>
{
    public async Task ExecuteAsync(UseCaseSubscriberData<JoinGameDto, PublicGameStateDto> data, CancellationToken cancellationToken)
    {
        var callerContext = _hubCallerContextRegistry.Context;
        var callerContextClients = _hubCallerContextRegistry.Clients;

        await _hubContext.Groups.AddToGroupAsync(callerContext.ConnectionId, data.UseCaseData.GameCode, cancellationToken);

        await callerContextClients
            .GroupExcept(data.UseCaseData.GameCode, callerContext.ConnectionId)
            .SendAsync("PlayerJoined", HubNotification.From(new PlayerJoinNotification
            {
                ConnectionId = callerContext.ConnectionId,
                Player = data.UseCaseResult.Data!.Players[callerContext.ConnectionId]
            }), cancellationToken);
    }
}
