namespace WebApi.Application.Features.Games.Services;

public interface ICreateGameService
{
    Task<string> CreateAsync(CancellationToken cancellationToken = default);
}