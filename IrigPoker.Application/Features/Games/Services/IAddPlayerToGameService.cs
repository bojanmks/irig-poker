using IrigPoker.Common.Features.Games.Joining.Models;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Application.Features.Games.Services;

public interface IAddPlayerToGameService
{
    Task<(string? PlayerId, GameState? GameState)> AddAsync(JoinGameRequest data, CancellationToken cancellationToken = default);
}
