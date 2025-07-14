using WebApi.Application.Games;
using WebApi.Common.DTO.Games;

namespace WebApi.Implementation.Games;

public class GetGameService(
    GameStore _gameStore    
) : IGetGameService
{
    public Task<GameDto?> GetAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        var gameState = _gameStore.GameStates.GetValueOrDefault(gameCode);
        return Task.FromResult(gameState);
    }
}
