using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.Services;

public class StartGameService(
    IGetGameService _getGameService
) : IStartGameService
{
    public async Task<IReadOnlyDictionary<string, List<Card>>> StartAsync(string gameCode, CancellationToken cancellationToken = default)
    {
        var game = await _getGameService.GetAsync(gameCode, cancellationToken) ?? throw new InvalidOperationException("Game not found");
        game.Start();
        game.CreateDeck();
        game.DealCardsToAllPlayers(1);
        game.ShufflePlayerOrder();
        game.NextTurn();

        return game.PlayerCards;
    }
}
