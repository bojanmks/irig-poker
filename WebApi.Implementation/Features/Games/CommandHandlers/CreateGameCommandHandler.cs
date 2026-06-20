using MediatR;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

public class CreateGameCommandHandler(
    ICreateGameService _createGameService
) : IRequestHandler<CreateGameCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateGameCommand command, CancellationToken cancellationToken)
    {
        string createdGameCode = await _createGameService.CreateAsync(cancellationToken);
        return createdGameCode;
    }
}
