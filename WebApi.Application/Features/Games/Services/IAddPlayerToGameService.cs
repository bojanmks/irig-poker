using WebApi.Common.Features.Games.Joining.Models;

namespace WebApi.Application.Features.Games.Services;

public interface IAddPlayerToGameService
{
    Task<string?> AddAsync(JoinGameDto data, CancellationToken cancellationToken = default);
}
