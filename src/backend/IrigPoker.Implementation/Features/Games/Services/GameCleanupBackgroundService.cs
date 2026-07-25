using Microsoft.Extensions.Hosting;
using IrigPoker.Application.Core.AppSettings;
using IrigPoker.Application.Features.Games.Services;

namespace IrigPoker.Implementation.Features.Games.Services;

public class GameCleanupBackgroundService(
    IGameCleanupService _gameCleanupService,
    AppSettings _appSettings
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = TimeSpan.FromMinutes(_appSettings.GameCleanupIntervalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(interval, stoppingToken);
            await _gameCleanupService.CleanupStaleGamesAsync(stoppingToken);
        }
    }
}
