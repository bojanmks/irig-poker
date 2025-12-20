using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.Services;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.UseCaseHandlers;

public class JoinGameUseCaseHandler(
    IAddPlayerToGameService _addPlayerToGameService,
    IGetGameService _getGameService
) : UseCaseHandler<JoinGameUseCase, JoinGameDto, PublicGameStateDto>
{
    public override async Task<Result<PublicGameStateDto>> HandleAsync(JoinGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        bool result = await _addPlayerToGameService.AddAsync(useCase.Data, cancellationToken);

        if (!result)
        {
            return Result<PublicGameStateDto>.Error();
        }

        var game = await _getGameService.GetAsync(useCase.Data.GameCode, cancellationToken);

        var response = new PublicGameStateDto
        {
            Players = game!.Players.ToDictionary()
        };

        return Result<PublicGameStateDto>.Success(response);
    }
}
