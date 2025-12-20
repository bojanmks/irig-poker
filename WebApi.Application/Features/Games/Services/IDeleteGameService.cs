namespace WebApi.Application.Features.Games.Services;

public interface IDeleteGameService
{
    Task DeleteAsync(string gameCode, CancellationToken cancellationToken = default);
}