namespace WebApi.Application.Games;

public interface IDeleteGameService
{
    Task DeleteAsync(string gameCode, CancellationToken cancellationToken = default);
}