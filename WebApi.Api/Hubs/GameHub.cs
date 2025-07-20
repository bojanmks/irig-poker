using Microsoft.AspNetCore.SignalR;
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
        await _mediator.Execute<JoinGameUseCase, JoinGameDto, Empty>(new JoinGameUseCase(data));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _mediator.Execute<DisconnectFromGameUseCase, string, DisconnectResult>(new DisconnectFromGameUseCase(Context.ConnectionId));
    }
}