using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Extensions;
using WebApi.Application.UseCases.Connection;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Hubs;

public class GameHub(
    UseCaseMediator _mediator
) : Hub
{
    public async Task<HubActionResponse<PublicGameStateDto>> JoinGame(JoinGameDto data)
    {
        data.ConnectionId = Context.ConnectionId;
        var result = await _mediator.Execute<JoinGameUseCase, JoinGameDto, PublicGameStateDto>(new JoinGameUseCase(data));
        return result.ToHubActionResponse();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _mediator.Execute<DisconnectUseCase, string, DisconnectResultDto>(new DisconnectUseCase(Context.ConnectionId));
    }
}