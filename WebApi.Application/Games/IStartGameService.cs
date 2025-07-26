namespace WebApi.Application.Games;

public interface IStartGameService
{
    Task StartAsync(string gameCode, CancellationToken cancellationToken = default);
}
