using MediatR;
using System.Collections.Frozen;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

public class JoinGameCommandHandler(
    IAddPlayerToGameService _addPlayerToGameService,
    IGetGameService _getGameService,
    ITranslator _translator
) : IRequestHandler<JoinGameCommand, Result<PublicGameStateDto>>
{
    public async Task<Result<PublicGameStateDto>> Handle(JoinGameCommand command, CancellationToken cancellationToken)
    {
        bool result = await _addPlayerToGameService.AddAsync(command.Data, cancellationToken);

        if (!result)
        {
            return Result<PublicGameStateDto>.Error();
        }

        var game = await _getGameService.GetAsync(command.Data.GameCode, cancellationToken);

        if (game is null)
        {
            return Result<PublicGameStateDto>.Error(_translator.Translate("game.notFound"));
        }

        var response = new PublicGameStateDto(game.HasStarted, game.Players.ToFrozenDictionary());

        return response;
    }
}
