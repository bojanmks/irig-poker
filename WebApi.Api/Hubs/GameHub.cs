using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Extensions;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Connection;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Hubs;

public class GameHub(
    UseCaseMediator _mediator
) : Hub
{
    public async Task<HubActionResponse<PublicGameStateDto>> JoinGame(HubActionRequest<JoinGameDto> request)
    {
        var result = await _mediator.Execute<JoinGameUseCase, JoinGameDto, PublicGameStateDto>(new JoinGameUseCase(request.Data), Context.ConnectionAborted);
        return result.ToHubActionResponse();
    }

    public async Task<HubActionResponse<Empty>> StartGame(HubActionRequest<Empty> _)
    {
        var result = await _mediator.Execute<StartGameUseCase, Empty, Empty>(new StartGameUseCase(Empty.Value), Context.ConnectionAborted);
        return result.ToHubActionResponse();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _mediator.Execute<DisconnectUseCase, string, DisconnectResultDto>(new DisconnectUseCase(Context.ConnectionId));
    }
}