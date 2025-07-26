using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Hubs;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;

namespace WebApi.Api.UseCaseSubscribers.Games;

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
            .SendAsync("PlayerJoined", new PlayerJoinNotification
            {
                ConnectionId = callerContext.ConnectionId,
                Player = data.UseCaseResult.Data!.Players[callerContext.ConnectionId]
            }, cancellationToken);
    }
}
