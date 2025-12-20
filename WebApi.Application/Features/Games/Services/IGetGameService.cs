using WebApi.Common.Features.Games.Models;

namespace WebApi.Application.Features.Games.Services;

public interface IGetGameService
{
    Task<GameDto?> GetAsync(string gameCode, CancellationToken cancellationToken = default);
}