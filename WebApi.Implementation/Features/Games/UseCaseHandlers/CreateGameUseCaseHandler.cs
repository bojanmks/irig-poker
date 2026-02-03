using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.Services;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Features.Games.UseCaseHandlers;

public class CreateGameUseCaseHandler(
    ICreateGameService _createGameService
) : UseCaseHandler<CreateGameUseCase, Empty, string>
{
    public override async Task<Result<string>> HandleAsync(CreateGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        string createdGameCode = await _createGameService.CreateAsync(cancellationToken);
        return createdGameCode;
    }
}