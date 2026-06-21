using System.Collections.Concurrent;
using WebApi.Application.Features.Games.Services;

namespace WebApi.Implementation.Features.Games.Services;

public class GameLockService : IGameLockService
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public void CreateLock(string gameCode)
    {
        _locks.TryAdd(gameCode, new SemaphoreSlim(1, 1));
    }

    public void RemoveLock(string gameCode)
    {
        _locks.TryRemove(gameCode, out var semaphore);
    }

    public async Task<IDisposable> AcquireLockAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        var semaphore = _locks.GetOrAdd(gameCode, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(cancellationToken);
        return new GameLockHandle(semaphore);
    }

    private sealed class GameLockHandle(
        SemaphoreSlim _semaphore
    ) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _semaphore?.Release();
        }
    }
}
