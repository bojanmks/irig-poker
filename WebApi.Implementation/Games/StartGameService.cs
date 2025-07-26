using WebApi.Application.Games;

namespace WebApi.Implementation.Games;

public class StartGameService(
    IGetGameService _getGameService    
) : IStartGameService
{
    public async Task StartAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        var game = await _getGameService.GetAsync(gameCode, cancellationToken);
        game!.HasStarted = true;
    }
}
