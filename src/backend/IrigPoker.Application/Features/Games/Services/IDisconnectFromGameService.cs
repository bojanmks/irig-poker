using IrigPoker.Common.Features.Games.Disconnecting.Models;

namespace IrigPoker.Application.Features.Games.Services;

public interface IDisconnectFromGameService
{
    Task<DisconnectResult> DisconnectAsync(string connectionId, CancellationToken cancellationToken = default);
}
