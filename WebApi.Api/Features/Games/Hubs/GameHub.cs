using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Models;
using WebApi.Api.Core.Result.Extensions;
using WebApi.Application.Features.Games.Commands;
using WebApi.Common.Core.Cqrs;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Api.Features.Games.Hubs;

public class GameHub(
    IMediator _mediator,
    TimeProvider _timeProvider
) : Hub
{
    public async Task<HubActionResponse<PublicGameStateDto>> JoinGame(HubActionRequest<JoinGameDto> request)
    {
        var result = await _mediator.Send(new JoinGameCommand(request.Data), Context.ConnectionAborted);

        if (result.IsSuccess)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, request.Data.GameCode, Context.ConnectionAborted);

            await Clients
                .GroupExcept(request.Data.GameCode, Context.ConnectionId)
                .SendAsync("PlayerJoined", HubNotification.From(new PlayerJoinNotification(
                    Context.ConnectionId,
                    result.Data.Players[Context.ConnectionId]
                ), _timeProvider), Context.ConnectionAborted);
        }

        return result.ToHubActionResponse();
    }

    public async Task<HubActionResponse<string>> StartGame(HubActionRequest<Empty> _)
    {
        var result = await _mediator.Send(new StartGameCommand(), Context.ConnectionAborted);

        if (result.IsSuccess)
        {
            await Clients
                .Group(result.Data)
                .SendAsync("GameStarted", HubNotification.From(Empty.Value, _timeProvider), Context.ConnectionAborted);
        }

        return result.ToHubActionResponse();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var result = await _mediator.Send(new DisconnectCommand(Context.ConnectionId));

        if (result.IsSuccess && !string.IsNullOrWhiteSpace(result.Data.GameCode))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, result.Data.GameCode);
            await Clients
                .GroupExcept(result.Data.GameCode, Context.ConnectionId)
                .SendAsync("PlayerLeft", HubNotification.From(Context.ConnectionId, _timeProvider));
        }
    }
}
