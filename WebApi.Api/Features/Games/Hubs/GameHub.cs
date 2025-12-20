using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Models;
using WebApi.Api.Core.Result.Extensions;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Common.Features.Games.Disconnecting.Models;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;
using WebApi.Implementation.Core.UseCases.Execution;

namespace WebApi.Api.Features.Games.Hubs;

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