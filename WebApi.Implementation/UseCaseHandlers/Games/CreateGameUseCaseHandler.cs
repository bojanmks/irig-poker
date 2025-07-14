using WebApi.Application.Games;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCaseHandlers.Games;

public class CreateGameUseCaseHandler(
    ICreateGameService _createGameService
) : UseCaseHandler<CreateGameUseCase, Empty, string>
{
    public override async Task<Result<string>> HandleAsync(CreateGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        string createdGameCode = await _createGameService.CreateAsync(cancellationToken);
        return Result<string>.Success(createdGameCode);
    }
}