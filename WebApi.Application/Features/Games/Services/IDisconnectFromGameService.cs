using WebApi.Common.Features.Games.Disconnecting.Models;

namespace WebApi.Application.Features.Games.Services;

public interface IDisconnectFromGameService
{
    Task<DisconnectResultDto> DisconnectAsync(string connectionId, CancellationToken cancellationToken = default);
}
