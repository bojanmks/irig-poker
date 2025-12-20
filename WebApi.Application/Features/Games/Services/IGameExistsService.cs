namespace WebApi.Application.Features.Games.Services;

public interface IGameExistsService
{
    Task<bool> ExistsAsync(string gameCode, CancellationToken cancellationToken = default);
}
