using MediatR;
using System.Collections.Frozen;
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
    ITranslator _translator
) : IRequestHandler<JoinGameCommand, Result<JoinGameResultDto>>
{
    public async Task<Result<JoinGameResultDto>> Handle(JoinGameCommand command, CancellationToken cancellationToken)
    {
        var playerId = await _addPlayerToGameService.AddAsync(command.Data, cancellationToken);

        if (playerId is null)
        {
            return Result<JoinGameResultDto>.Error(_translator.Translate("game.failedToJoin"));
        }

        var game = await _getGameService.GetAsync(command.Data.GameCode, cancellationToken);

        if (game is null)
        {
            return Result<JoinGameResultDto>.Error(_translator.Translate("game.notFound"));
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
