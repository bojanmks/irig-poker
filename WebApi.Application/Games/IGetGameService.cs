using WebApi.Common.DTO.Games;

namespace WebApi.Application.Games;

public interface IGetGameService
{
    Task<GameDto?> GetAsync(string gameCode, CancellationToken cancellationToken = default);
}