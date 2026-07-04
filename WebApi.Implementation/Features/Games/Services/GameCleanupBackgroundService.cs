using Microsoft.Extensions.Hosting;
using WebApi.Application.Core.AppSettings;
using WebApi.Application.Features.Games.Services;

namespace WebApi.Implementation.Features.Games.Services;

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
