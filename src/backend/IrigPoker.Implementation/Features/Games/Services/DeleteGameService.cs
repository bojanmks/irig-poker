using IrigPoker.Application.Features.Games.Services;
using IrigPoker.Implementation.Features.Games.Stores;

namespace IrigPoker.Implementation.Features.Games.Services;

public class DeleteGameService(
    GameStore _gameStore,
    PlayersGamesMap _playersGamesMap,
    IGameLockService _gameLockService
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
            _playersGamesMap.PlayerIdToConnectionId.TryRemove(player.Key, out _);
        }

        _gameLockService.RemoveLock(gameCode);

        return Task.CompletedTask;
    }
}
