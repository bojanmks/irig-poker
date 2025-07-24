using WebApi.Common.DTO.Games;

namespace WebApi.Application.Games;

public interface IDisconnectFromGameService
{
    Task<DisconnectResultDto> DisconnectAsync(string connectionId, CancellationToken cancellationToken = default);
}
