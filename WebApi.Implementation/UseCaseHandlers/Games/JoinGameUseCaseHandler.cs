using WebApi.Application.Games;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCaseHandlers.Games;

public class JoinGameUseCaseHandler(
    IAddPlayerToGameService _addPlayerToGameService
) : UseCaseHandler<JoinGameUseCase, JoinGameDto, Empty>
{
    public override async Task<Result<Empty>> HandleAsync(JoinGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        bool result = await _addPlayerToGameService.AddAsync(useCase.Data, cancellationToken);

        if (!result)
        {
            return Result<Empty>.Error();
        }

        return Result<Empty>.Success();
    }
}
