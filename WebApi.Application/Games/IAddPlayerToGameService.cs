using WebApi.Common.DTO.Games;

namespace WebApi.Application.Games;

public interface IAddPlayerToGameService
{
    Task<bool> AddAsync(JoinGameDto data, CancellationToken cancellationToken = default);
}
