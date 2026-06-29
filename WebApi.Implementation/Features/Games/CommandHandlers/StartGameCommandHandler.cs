using MediatR;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.AppSettings;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

public class StartGameCommandHandler(
    IApplicationUserResolver _applicationUserResolver,
    IStartGameService _startGameService,
    IGetGameService _getGameService,
    IGameLockService _gameLockService,
    ITranslator _translator,
    AppSettings _appSettings
) : IRequestHandler<StartGameCommand, Result<StartGameResult>>
{
    public async Task<Result<StartGameResult>> Handle(StartGameCommand command, CancellationToken cancellationToken)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(applicationUser.GameCode))
        {
            return Result<StartGameResult>.Error(_translator.Translate("user.notInGame"));
        }

        string gameCode = applicationUser.GameCode;

        using (await _gameLockService.AcquireLockAsync(gameCode, cancellationToken))
        {
            var game = await _getGameService.GetAsync(gameCode, cancellationToken);

            if (game is null)
            {
                return Result<StartGameResult>.Error(_translator.Translate("game.notFound"));
            }

            if (game.HasStarted)
            {
                return Result<StartGameResult>.Error(_translator.Translate("game.alreadyStarted"));
            }

            if (game.Players.Count < _appSettings.MinPlayersPerGame)
            {
                return Result<StartGameResult>.Error(_translator.Translate("game.notEnoughPlayers"));
            }

            if (game.Players.Count > _appSettings.MaxPlayersPerGame)
            {
                return Result<StartGameResult>.Error(_translator.Translate("game.tooManyPlayers"));
            }

            var playerCards = await _startGameService.StartAsync(gameCode, cancellationToken);

            var gameState = new PublicGameState(
                game!.GameCode,
                game.HasStarted,
                game.Players,
                game.PlayerOrder,
                game.CurrentTurnPlayerId,
                null,
                null,
                null
            );

            return new StartGameResult(gameState, playerCards);
        }
    }
}
