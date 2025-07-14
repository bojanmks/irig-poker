namespace WebApi.Application.Games;

public interface IGameExistsService
{
    Task<bool> ExistsAsync(string gameCode, CancellationToken cancellationToken = default);
}
