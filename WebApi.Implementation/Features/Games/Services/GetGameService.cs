using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.Services;

public class GetGameService(
    GameStore _gameStore
) : IGetGameService
{
    public Task<GameDto?> GetAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        var gameState = _gameStore.Games.GetValueOrDefault(gameCode);
        return Task.FromResult(gameState);
    }
}
