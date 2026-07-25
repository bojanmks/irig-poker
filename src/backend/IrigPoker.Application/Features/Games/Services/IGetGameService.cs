using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Application.Features.Games.Services;

public interface IGetGameService
{
    Task<GameState?> GetAsync(string gameCode, CancellationToken cancellationToken = default);
}