using MediatR;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Disconnecting.Models;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

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
