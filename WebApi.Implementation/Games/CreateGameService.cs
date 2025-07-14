using System.Security.Cryptography;
using System.Text;
using WebApi.Application.Games;
using WebApi.Common.DTO.Games;

namespace WebApi.Implementation.Games;

public class CreateGameService(
    GameStore _gameStore    
) : ICreateGameService
{
    public Task<string> CreateAsync(CancellationToken cancellationToken = default)
    {
        string gameCode = MakeGameCode();

        var gameState = new GameDto
        {
            GameCode = gameCode
        };

        while (!_gameStore.GameStates.TryAdd(gameCode, gameState))
        {
            gameCode = MakeGameCode();
            gameState.GameCode = gameCode;
        }

        return Task.FromResult(gameCode);
    }

    private string MakeGameCode()
    {
        const string ALLOWED_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const int LENGTH = 20;

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