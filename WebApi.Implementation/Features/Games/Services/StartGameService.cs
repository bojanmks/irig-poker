using WebApi.Application.Features.Games.Services;

namespace WebApi.Implementation.Features.Games.Services;

public class StartGameService(
    IGetGameService _getGameService
) : IStartGameService
{
    public async Task StartAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        var game = await _getGameService.GetAsync(gameCode, cancellationToken);
        game!.HasStarted = true;

        var playerIds = game.Players.Keys.ToArray();
        Random.Shared.Shuffle(playerIds);
        game.PlayerOrder = [.. playerIds];
        game.CurrentTurnPlayerId = playerIds[0];
    }
}
