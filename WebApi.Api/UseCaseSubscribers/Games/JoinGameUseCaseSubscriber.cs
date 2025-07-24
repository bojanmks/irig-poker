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
    public async Task OnUseCaseExecuted(UseCaseSubscriberData<JoinGameDto, PublicGameStateDto> data)
    {
        var callerContext = _hubCallerContextRegistry.Context;
        var callerContextClients = _hubCallerContextRegistry.Clients;

        await _hubContext.Groups.AddToGroupAsync(callerContext.ConnectionId, data.UseCaseData.GameCode);

        await callerContextClients
            .GroupExcept(data.UseCaseData.GameCode, callerContext.ConnectionId)
            .SendAsync("PlayerJoined", new PlayerJoinNotification
            {
                ConnectionId = callerContext.ConnectionId,
                Player = data.UseCaseResult.Data!.Players[callerContext.ConnectionId]
            });
    }
}
