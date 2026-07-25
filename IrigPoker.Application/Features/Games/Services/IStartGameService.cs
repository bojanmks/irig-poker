using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Application.Features.Games.Services;

public interface IStartGameService
{
    Task<IReadOnlyDictionary<string, List<Card>>> StartAsync(string gameCode, CancellationToken cancellationToken = default);
}
