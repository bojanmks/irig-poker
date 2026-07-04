using MediatR;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

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

            var publicGameState = new PublicGameState(
                gameState.GameCode,
                gameState.HasStarted,
                gameState.Players,
                gameState.PlayerOrder,
                gameState.CurrentTurnPlayerId,
                null,
                null,
                null,
                null,
                gameState.MaxCardCount,
                gameState.RoundHistory
            );

            return new JoinGameResult(playerId, publicGameState);
        }
    }
}
