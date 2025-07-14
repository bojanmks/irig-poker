using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Extensions;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Hubs;

public class GameHub(
    UseCaseMediator _mediator    
) : Hub
{
    public async Task JoinGame(JoinGameDto data)
    {
        data.ConnectionId = Context.ConnectionId;
        var result = await _mediator.Execute<JoinGameUseCase, JoinGameDto, Empty>(new JoinGameUseCase(data));

        if (!result.IsSuccess)
        {
            await Clients.Caller.SendAsync("JoinFailed", result.ToHubActionResponse());
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, data.GameCode);
        await Clients.Group(data.GameCode).SendAsync("PlayerJoined", Context.ConnectionId);
    }
}