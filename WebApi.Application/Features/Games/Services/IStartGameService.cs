using WebApi.Common.Features.Games.Models;

namespace WebApi.Application.Features.Games.Services;

public interface IStartGameService
{
    Task<IReadOnlyDictionary<string, List<Card>>> StartAsync(string gameCode, CancellationToken cancellationToken = default);
}
