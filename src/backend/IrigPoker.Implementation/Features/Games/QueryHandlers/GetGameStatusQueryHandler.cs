using MediatR;
using IrigPoker.Application.Features.Games.Queries;
using IrigPoker.Application.Features.Games.Services;
using IrigPoker.Common.Core.Result.Models;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Implementation.Features.Games.QueryHandlers;

public class GetGameStatusQueryHandler(
    IGetGameService _getGameService
) : IRequestHandler<GetGameStatusQuery, Result<GameStatusResult>>
{
    public async Task<Result<GameStatusResult>> Handle(GetGameStatusQuery query, CancellationToken cancellationToken)
    {
        var game = await _getGameService.GetAsync(query.GameCode, cancellationToken);

        return game is null
            ? new GameStatusResult(false, false)
            : new GameStatusResult(true, game.HasStarted);
    }
}
