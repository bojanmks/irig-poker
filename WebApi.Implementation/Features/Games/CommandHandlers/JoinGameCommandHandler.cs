using MediatR;
using System.Collections.Frozen;
using WebApi.Application.Core.AppSettings;
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
    ITranslator _translator,
    AppSettings _appSettings
) : IRequestHandler<JoinGameCommand, Result<JoinGameResultDto>>
{
    public async Task<Result<JoinGameResultDto>> Handle(JoinGameCommand command, CancellationToken cancellationToken)
    {
        using (await _gameLockService.AcquireLockAsync(command.Data.GameCode, cancellationToken))
        {
            var game = await _getGameService.GetAsync(command.Data.GameCode, cancellationToken);

            if (game is null)
            {
                return Result<JoinGameResultDto>.Error(_translator.Translate("game.notFound"));
            }

            if (game.HasStarted)
            {
                return Result<JoinGameResultDto>.Error(_translator.Translate("game.alreadyStarted"));
            }

            if (game.Players.Count >= _appSettings.MaxPlayersPerGame)
            {
                return Result<JoinGameResultDto>.Error(_translator.Translate("game.isFull"));
            }

            var playerId = await _addPlayerToGameService.AddAsync(command.Data, cancellationToken);

            if (playerId is null)
            {
                return Result<JoinGameResultDto>.Error(_translator.Translate("game.failedToJoin"));
            }

            var gameState = new PublicGameStateDto(
                game.GameCode,
                game.HasStarted,
                game.Players.ToFrozenDictionary(),
                game.PlayerOrder.AsReadOnly(),
                game.CurrentTurnPlayerId
            );

            return new JoinGameResultDto(playerId, gameState);
        }
    }
}
