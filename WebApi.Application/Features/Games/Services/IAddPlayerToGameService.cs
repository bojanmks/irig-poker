using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Application.Features.Games.Services;

public interface IAddPlayerToGameService
{
    Task<(string? PlayerId, GameState? GameState)> AddAsync(JoinGameRequest data, CancellationToken cancellationToken = default);
}
