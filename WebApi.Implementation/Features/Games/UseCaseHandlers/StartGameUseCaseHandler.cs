using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.Services;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Features.Games.UseCaseHandlers;

public class StartGameUseCaseHandler(
    IApplicationUserResolver _applicationUserResolver,
    IStartGameService _startGameService
) : UseCaseHandler<StartGameUseCase, Empty, Empty>
{
    public override async Task<Result<Empty>> HandleAsync(StartGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);
        await _startGameService.StartAsync(applicationUser.GameCode!, cancellationToken);
        return Empty.Value;
    }
}
