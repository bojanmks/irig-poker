using WebApi.Common.DTO.Games;

namespace WebApi.Application.Games;

public interface IDisconnectFromGameService
{
    Task<DisconnectResult> DisconnectAsync(string connectionId, CancellationToken cancellationToken = default);
}
