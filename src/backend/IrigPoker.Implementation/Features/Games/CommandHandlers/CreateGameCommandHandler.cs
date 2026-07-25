using MediatR;
using IrigPoker.Application.Features.Games.Commands;
using IrigPoker.Application.Features.Games.Services;
using IrigPoker.Common.Core.Result.Models;

namespace IrigPoker.Implementation.Features.Games.CommandHandlers;

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
