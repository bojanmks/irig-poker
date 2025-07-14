using WebApi.Application.Games;

namespace WebApi.Implementation.Games;

public class DeleteGameService(
    GameStore _gameStore    
) : IDeleteGameService
{
    public Task DeleteAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        _gameStore.GameStates.Remove(gameCode, out _);
        return Task.CompletedTask;
    }
}
