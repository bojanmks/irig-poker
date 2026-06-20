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
        var result = await _mediator.ExecuteAsync<JoinGameUseCase, JoinGameDto, PublicGameStateDto>(new JoinGameUseCase(request.Data), Context.ConnectionAborted);

        if (result.IsSuccess)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, request.Data.GameCode, Context.ConnectionAborted);

            await Clients
                .GroupExcept(request.Data.GameCode, Context.ConnectionId)
                .SendAsync("PlayerJoined", HubNotification.From(new PlayerJoinNotification
                {
                    ConnectionId = Context.ConnectionId,
                    Player = result.Data.Players[Context.ConnectionId]
                }), Context.ConnectionAborted);
        }

        return result.ToHubActionResponse();
    }

    public async Task<HubActionResponse<string>> StartGame(HubActionRequest<Empty> _)
    {
        var result = await _mediator.ExecuteAsync<StartGameUseCase, Empty, string>(new StartGameUseCase(Empty.Value), Context.ConnectionAborted);

        if (result.IsSuccess)
        {
            await Clients
                .Group(result.Data)
                .SendAsync("GameStarted", HubNotification.From(Empty.Value), Context.ConnectionAborted);
        }

        return result.ToHubActionResponse();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var result = await _mediator.ExecuteAsync<DisconnectUseCase, string, DisconnectResultDto>(new DisconnectUseCase(Context.ConnectionId));

        if (result.IsSuccess && !string.IsNullOrWhiteSpace(result.Data.GameCode))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, result.Data.GameCode);
            await Clients
                .GroupExcept(result.Data.GameCode, Context.ConnectionId)
                .SendAsync("PlayerLeft", HubNotification.From(Context.ConnectionId));
        }

    }
}