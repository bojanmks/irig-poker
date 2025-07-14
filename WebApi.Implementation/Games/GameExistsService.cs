using WebApi.Application.Games;

namespace WebApi.Implementation.Games;

public class GameExistsService(
    GameStore _gameStore    
) : IGameExistsService
{
    public Task<bool> ExistsAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        bool gameExist = _gameStore.GameStates.ContainsKey(gameCode);
        return Task.FromResult(gameExist);
    }
}
