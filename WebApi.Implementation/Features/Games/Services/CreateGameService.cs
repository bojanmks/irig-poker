using System.Security.Cryptography;
using System.Text;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.Services;

public class CreateGameService(
    GameStore _gameStore,
    IGameLockService _gameLockService
) : ICreateGameService
{
    public Task<string> CreateAsync(CancellationToken cancellationToken = default)
    {
        GameState gameState;

        do
        {
            gameState = new GameState
            {
                GameCode = MakeGameCode()
            };
        }
        while (!_gameStore.Games.TryAdd(gameState.GameCode, gameState));

        _gameLockService.CreateLock(gameState.GameCode);

        return Task.FromResult(gameState.GameCode);
    }

    private string MakeGameCode()
    {
        const string ALLOWED_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const int LENGTH = 10;

        var code = new StringBuilder(LENGTH);
        using var rng = RandomNumberGenerator.Create();
        var byteBuffer = new byte[1];

        while (code.Length < LENGTH)
        {
            rng.GetBytes(byteBuffer);
            var num = byteBuffer[0] % ALLOWED_CHARACTERS.Length;
            code.Append(ALLOWED_CHARACTERS[num]);
        }

        return code.ToString();
    }
}
