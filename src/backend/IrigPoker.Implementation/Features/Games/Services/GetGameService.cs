using IrigPoker.Application.Features.Games.Services;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Implementation.Features.Games.Services;

public class GetGameService(
    GameStore _gameStore
) : IGetGameService
{
    public Task<GameState?> GetAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        var gameState = _gameStore.Games.GetValueOrDefault(gameCode);
        return Task.FromResult(gameState);
    }
}
