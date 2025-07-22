using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Hubs;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;

namespace WebApi.Api.UseCaseSubscribers.Games;

public class JoinGameUseCaseSubscriber(
    IHubContext<GameHub> _hubContext,
    HubCallerContextRegistry _hubCallerContextRegistry
) : IUseCaseSubscriber<JoinGameUseCase, JoinGameDto, PublicGameState>
{
    public async Task OnUseCaseExecuted(UseCaseSubscriberData<JoinGameDto, PublicGameState> data)
    {
        var callerContext = _hubCallerContextRegistry.Context;
        var callerContextClients = _hubCallerContextRegistry.Clients;

        await _hubContext.Groups.AddToGroupAsync(callerContext.ConnectionId, data.UseCaseData.GameCode);

        await callerContextClients.Group(data.UseCaseData.GameCode).SendAsync("PlayerJoined", new PlayerJoinNotification
        {
            ConnectionId = callerContext.ConnectionId,
            Username = data.UseCaseData.Username
        });
    }
}
