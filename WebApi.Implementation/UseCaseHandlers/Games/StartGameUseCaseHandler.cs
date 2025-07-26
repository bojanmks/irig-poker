using WebApi.Application.ApplicationUsers;
using WebApi.Application.Games;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCaseHandlers.Games;

public class StartGameUseCaseHandler(
    IApplicationUserResolver _applicationUserResolver,
    IStartGameService _startGameService
) : UseCaseHandler<StartGameUseCase, Empty, Empty>
{
    public override async Task<Result<Empty>> HandleAsync(StartGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);
        await _startGameService.StartAsync(applicationUser.GameCode, cancellationToken);
        return Result<Empty>.Success(Empty.Value);
    }
}
