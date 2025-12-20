namespace WebApi.Application.Features.Games.Services;

public interface IStartGameService
{
    Task StartAsync(string gameCode, CancellationToken cancellationToken = default);
}
