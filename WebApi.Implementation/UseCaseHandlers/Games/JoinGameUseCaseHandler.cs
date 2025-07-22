using WebApi.Application.Games;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCaseHandlers.Games;

public class JoinGameUseCaseHandler(
    IAddPlayerToGameService _addPlayerToGameService,
    IGetGameService _getGameService
) : UseCaseHandler<JoinGameUseCase, JoinGameDto, PublicGameState>
{
    public override async Task<Result<PublicGameState>> HandleAsync(JoinGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        bool result = await _addPlayerToGameService.AddAsync(useCase.Data, cancellationToken);

        if (!result)
        {
            return Result<PublicGameState>.Error();
        }

        var game = await _getGameService.GetAsync(useCase.Data.GameCode, cancellationToken);

        var response = new PublicGameState
        {
            Players = game!.Players.ToDictionary()
        };

        return Result<PublicGameState>.Success(response);
    }
}
