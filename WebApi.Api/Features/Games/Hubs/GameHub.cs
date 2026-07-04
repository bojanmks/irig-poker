using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Models;
using WebApi.Api.Core.Result.Extensions;
using WebApi.Application.Features.Games.Commands;
using WebApi.Common.Core.Cqrs;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;
using WebApi.Common.Features.Games.Winning.Models;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Api.Features.Games.Hubs;

public class GameHub(
    IMediator _mediator,
    TimeProvider _timeProvider,
    PlayersGamesMap _playersGamesMap
) : Hub
{
    public async Task<HubActionResponse<JoinGameResult>> JoinGame(HubActionRequest<JoinGameRequest> request)
    {
        var result = await _mediator.Send(new JoinGameCommand(request.Data), Context.ConnectionAborted);

        if (result.IsSuccess)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, request.Data.GameCode, Context.ConnectionAborted);

            var player = result.Data.GameState.Players[result.Data.PlayerId];

            await Clients
                .GroupExcept(request.Data.GameCode, Context.ConnectionId)
                .SendAsync("PlayerJoined", HubNotification.From(new PlayerJoinNotification(
                    player,
                    result.Data.GameState.PlayerOrder
                ), _timeProvider));

            await Clients
                .Group(request.Data.GameCode)
                .SendAsync("GameStateUpdated", HubNotification.From(
                    result.Data.GameState, _timeProvider
                ));
        }

        return result.ToHubActionResponse();
    }

    public async Task<HubActionResponse<StartGameResult>> StartGame(HubActionRequest<Empty> _)
    {
        var result = await _mediator.Send(new StartGameCommand(), Context.ConnectionAborted);

        if (result.IsSuccess)
        {
            var startGameResult = result.Data;
            var gameCode = startGameResult.GameState.GameCode;

            await Clients
                .Group(gameCode)
                .SendAsync("GameStarted", HubNotification.From(startGameResult.GameState, _timeProvider));

            var tasks = new List<Task>();
            foreach (var (playerId, cards) in startGameResult.PlayerCards)
            {
                if (_playersGamesMap.TryGetConnectionId(playerId, out var connectionId))
                {
                    tasks.Add(
                        Clients.Client(connectionId)
                            .SendAsync("CardsDealt", HubNotification.From(
                                new CardsDealtNotification(cards),
                                _timeProvider
                            ))
                    );
                }
            }

            await Task.WhenAll(tasks);
        }

        return result.ToHubActionResponse();
    }

    public async Task<HubActionResponse<ClaimResult>> ClaimHand(HubActionRequest<ClaimHandRequest> request)
    {
        var result = await _mediator.Send(new ClaimHandCommand(request.Data), Context.ConnectionAborted);

        if (result.IsSuccess)
        {
            var claimResult = result.Data;
            var gameCode = claimResult.GameState.GameCode;

            await Clients.Group(gameCode)
                .SendAsync("ClaimMade", HubNotification.From(
                    claimResult.ClaimNotification, _timeProvider
                ));

            await Clients.Group(gameCode)
                .SendAsync("GameStateUpdated", HubNotification.From(
                    claimResult.GameState, _timeProvider
                ));
        }

        return result.ToHubActionResponse();
    }

    public async Task<HubActionResponse<CallBluffResult>> CallBluff(HubActionRequest<CallBluffRequest> _)
    {
        var result = await _mediator.Send(new CallBluffCommand(), Context.ConnectionAborted);

        if (result.IsSuccess)
        {
            var callBluffResult = result.Data;
            var gameCode = callBluffResult.GameCode;

            await Clients.Group(gameCode)
                .SendAsync("RoundResolved", HubNotification.From(
                    callBluffResult.Resolution, _timeProvider
                ));

            var tasks = new List<Task>();

            if (callBluffResult.WinnerPlayerId is not null)
            {
                tasks.Add(
                    Clients.Group(gameCode)
                        .SendAsync("GameWon", HubNotification.From(
                            new WinnerNotification(callBluffResult.WinnerPlayerId, callBluffResult.WinnerUsername!),
                            _timeProvider
                        ))
                );
            }
            else if (callBluffResult.UpdatedGameState is not null)
            {
                tasks.Add(
                    Clients.Group(gameCode)
                        .SendAsync("GameStateUpdated", HubNotification.From(
                            callBluffResult.UpdatedGameState, _timeProvider
                        ))
                );
            }

            foreach (var (playerId, cards) in callBluffResult.PlayerCards)
            {
                if (_playersGamesMap.TryGetConnectionId(playerId, out var connectionId))
                {
                    tasks.Add(
                        Clients.Client(connectionId)
                            .SendAsync("CardsDealt", HubNotification.From(
                                new CardsDealtNotification(cards),
                                _timeProvider
                            ))
                    );
                }
            }

            await Task.WhenAll(tasks);
        }

        return result.ToHubActionResponse();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var result = await _mediator.Send(new DisconnectCommand(Context.ConnectionId));

        if (result.IsSuccess && !string.IsNullOrWhiteSpace(result.Data.GameCode))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, result.Data.GameCode);

            var tasks = new List<Task>
            {
                Clients
                    .GroupExcept(result.Data.GameCode, Context.ConnectionId)
                    .SendAsync("PlayerLeft", HubNotification.From(result.Data.PlayerId!, _timeProvider))
            };

            if (result.Data.UpdatedGameState is not null)
            {
                tasks.Add(
                    Clients
                        .Group(result.Data.GameCode)
                        .SendAsync("GameStateUpdated", HubNotification.From(
                            result.Data.UpdatedGameState, _timeProvider
                        ))
                );
            }

            if (!string.IsNullOrWhiteSpace(result.Data.WinnerPlayerId))
            {
                tasks.Add(
                    Clients
                        .Group(result.Data.GameCode)
                        .SendAsync("GameWon", HubNotification.From(
                            new WinnerNotification(result.Data.WinnerPlayerId, result.Data.WinnerUsername!),
                            _timeProvider
                        ))
                );
            }

            await Task.WhenAll(tasks);
        }
    }
}
