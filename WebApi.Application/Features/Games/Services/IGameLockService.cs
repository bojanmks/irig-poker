namespace WebApi.Application.Features.Games.Services;

public interface IGameLockService
{
    void CreateLock(string gameCode);
    void RemoveLock(string gameCode);
    Task<IDisposable> AcquireLockAsync(string gameCode, CancellationToken cancellationToken = default);
}
