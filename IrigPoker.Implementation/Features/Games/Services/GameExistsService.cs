using IrigPoker.Application.Features.Games.Services;

namespace IrigPoker.Implementation.Features.Games.Services;

public class GameExistsService(
    GameStore _gameStore
) : IGameExistsService
{
    public Task<bool> ExistsAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        bool gameExist = _gameStore.Games.ContainsKey(gameCode);
        return Task.FromResult(gameExist);
    }
}
