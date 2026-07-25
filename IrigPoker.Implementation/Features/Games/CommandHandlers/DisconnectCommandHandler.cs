using MediatR;
using IrigPoker.Application.Features.Games.Commands;
using IrigPoker.Application.Features.Games.Services;
using IrigPoker.Common.Core.Result.Models;
using IrigPoker.Common.Features.Games.Disconnecting.Models;

namespace IrigPoker.Implementation.Features.Games.CommandHandlers;

public class DisconnectCommandHandler(
    IDisconnectFromGameService _disconnectFromGameService
) : IRequestHandler<DisconnectCommand, Result<DisconnectResult>>
{
    public async Task<Result<DisconnectResult>> Handle(DisconnectCommand command, CancellationToken cancellationToken)
    {
        var result = await _disconnectFromGameService.DisconnectAsync(command.ConnectionId, cancellationToken);
        return result;
    }
}
