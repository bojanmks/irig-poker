namespace WebApi.Application.Games;

public interface ICreateGameService
{
    Task<string> CreateAsync(CancellationToken cancellationToken = default);
}