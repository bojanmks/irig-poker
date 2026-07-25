using MediatR;
using IrigPoker.Application.Core.Localization;
using IrigPoker.Application.Features.Games.Commands;
using IrigPoker.Application.Features.Games.Services;
using IrigPoker.Common.Core.Result.Models;
using IrigPoker.Common.Features.Games.Joining.Models;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Implementation.Features.Games.CommandHandlers;

public class JoinGameCommandHandler(
    IAddPlayerToGameService _addPlayerToGameService,
    IGetGameService _getGameService,
    IGameLockService _gameLockService,
    ITranslator _translator
) : IRequestHandler<JoinGameCommand, Result<JoinGameResult>>
{
    public async Task<Result<JoinGameResult>> Handle(JoinGameCommand command, CancellationToken cancellationToken)
    {
        using (await _gameLockService.AcquireLockAsync(command.Data.GameCode, cancellationToken))
        {
            var game = await _getGameService.GetAsync(command.Data.GameCode, cancellationToken);

            if (game is null)
            {
                return Result<JoinGameResult>.Error(_translator.Translate("game.notFound"));
            }

            if (game.HasStarted)
            {
                return Result<JoinGameResult>.Error(_translator.Translate("game.alreadyStarted"));
            }

            if (game.Players.Count >= GameState.MaxPlayers)
            {
                return Result<JoinGameResult>.Error(_translator.Translate("game.isFull"));
            }

            var (playerId, gameState) = await _addPlayerToGameService.AddAsync(command.Data, cancellationToken);

            if (playerId is null || gameState is null)
            {
                return Result<JoinGameResult>.Error(_translator.Translate("game.failedToJoin"));
            }

            return new JoinGameResult(playerId, PublicGameState.FromGameState(gameState));
        }
    }
}
