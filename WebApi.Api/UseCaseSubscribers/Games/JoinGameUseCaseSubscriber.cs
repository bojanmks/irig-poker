using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Extensions;
using WebApi.Api.Hubs;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;

namespace WebApi.Api.UseCaseSubscribers.Games;

public class JoinGameUseCaseSubscriber(
    IHubContext<GameHub> _hubContext,
    HubContextRegistry _hubContextRegistry
) : IUseCaseSubscriber<JoinGameUseCase, JoinGameDto, Empty>
{
    public async Task OnUseCaseExecuted(UseCaseSubscriberData<JoinGameDto, Empty> data)
    {
        var context = _hubContextRegistry.Context;
        var clients = _hubContextRegistry.Clients;

        if (!data.Response.IsSuccess)
        {
            await clients.Caller.SendAsync("JoinFailed", data.Response.ToHubActionResponse());
            return;
        }

        await _hubContext.Groups.AddToGroupAsync(context.ConnectionId, data.UseCaseData.GameCode);

        await clients.Group(data.UseCaseData.GameCode).SendAsync("PlayerJoined", new PlayerJoinNotification
        {
            ConnectionId = context.ConnectionId,
            Username = data.UseCaseData.Username
        });
    }
}
