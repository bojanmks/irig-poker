namespace WebApi.Application.Features.Games.Services;

public interface IGameCleanupService
{
    Task CleanupStaleGamesAsync(CancellationToken cancellationToken = default);
}
