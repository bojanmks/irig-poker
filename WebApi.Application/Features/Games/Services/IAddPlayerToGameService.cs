using WebApi.Common.Features.Games.Joining.Models;

namespace WebApi.Application.Features.Games.Services;

public interface IAddPlayerToGameService
{
    Task<string?> AddAsync(JoinGameRequest data, CancellationToken cancellationToken = default);
}
