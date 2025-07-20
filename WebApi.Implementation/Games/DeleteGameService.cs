using WebApi.Application.Games;
using WebApi.Implementation.Players;

namespace WebApi.Implementation.Games;

public class DeleteGameService(
    GameStore _gameStore,
    PlayersGamesMap _playersGamesMap
) : IDeleteGameService
{
    public Task DeleteAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        if (!_gameStore.Games.Remove(gameCode, out var game))
        {
            return Task.CompletedTask;
        }

        foreach (var player in game.Players)
        {
            _playersGamesMap.Map.TryRemove(player.Key, out _);
        }

        return Task.CompletedTask;
    }
}
