using IrigPoker.Application.Core.AppSettings;
using IrigPoker.Application.Features.Games.Services;

namespace IrigPoker.Implementation.Features.Games.Services;

public class GameCleanupService(
    GameStore _gameStore,
    IDeleteGameService _deleteGameService,
    IGameLockService _gameLockService,
    AppSettings _appSettings
) : IGameCleanupService
{
    public async Task CleanupStaleGamesAsync(CancellationToken cancellationToken = default)
    {
        var staleTimeout = TimeSpan.FromMinutes(_appSettings.StaleGameTimeoutMinutes);
        var cutoff = DateTimeOffset.UtcNow - staleTimeout;

        foreach (var (gameCode, game) in _gameStore.Games)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            if (game.LastActivityAt >= cutoff)
                continue;

            using (await _gameLockService.AcquireLockAsync(gameCode, cancellationToken))
            {
                if (game.LastActivityAt >= cutoff)
                    continue;

                await _deleteGameService.DeleteAsync(gameCode, cancellationToken);
            }
        }
    }
}
