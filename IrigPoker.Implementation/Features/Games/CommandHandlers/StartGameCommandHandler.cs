using MediatR;
using IrigPoker.Application.Core.ApplicationUsers;
using IrigPoker.Application.Core.AppSettings;
using IrigPoker.Application.Core.Localization;
using IrigPoker.Application.Features.Games.Commands;
using IrigPoker.Application.Features.Games.Services;
using IrigPoker.Common.Core.Result.Models;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Implementation.Features.Games.CommandHandlers;

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

            if (game.Players.Count > GameState.MaxPlayers)
            {
                return Result<StartGameResult>.Error(_translator.Translate("game.tooManyPlayers"));
            }

            var playerCards = await _startGameService.StartAsync(gameCode, cancellationToken);

            return new StartGameResult(PublicGameState.FromGameState(game), playerCards);
        }
    }
}
