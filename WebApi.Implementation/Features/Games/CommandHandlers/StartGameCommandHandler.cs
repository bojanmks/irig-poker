using MediatR;
using System.Collections.Frozen;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

public class StartGameCommandHandler(
    IApplicationUserResolver _applicationUserResolver,
    IStartGameService _startGameService,
    IGetGameService _getGameService
) : IRequestHandler<StartGameCommand, Result<PublicGameStateDto>>
{
    public async Task<Result<PublicGameStateDto>> Handle(StartGameCommand command, CancellationToken cancellationToken)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        string gameCode = applicationUser.GameCode!;
        await _startGameService.StartAsync(gameCode, cancellationToken);

        var game = await _getGameService.GetAsync(gameCode, cancellationToken);

        var gameState = new PublicGameStateDto(
            game!.GameCode,
            game.HasStarted,
            game.Players.ToFrozenDictionary(),
            game.PlayerOrder.AsReadOnly(),
            game.CurrentTurnPlayerId
        );

        return gameState;
    }
}
