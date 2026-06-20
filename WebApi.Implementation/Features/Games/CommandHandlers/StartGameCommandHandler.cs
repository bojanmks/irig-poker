using MediatR;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

public class StartGameCommandHandler(
    IApplicationUserResolver _applicationUserResolver,
    IStartGameService _startGameService
) : IRequestHandler<StartGameCommand, Result<string>>
{
    public async Task<Result<string>> Handle(StartGameCommand command, CancellationToken cancellationToken)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        string gameCode = applicationUser.GameCode!;
        await _startGameService.StartAsync(gameCode, cancellationToken);

        return gameCode;
    }
}
