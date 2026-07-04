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
        game.MarkStarted();
        game.UpdateCardCountThreshold();
        game.InitializeCardCounts();
        game.CreateDeck();
        game.DealCardsToAllPlayers();
        game.ShufflePlayerOrder();
        game.StartNewRound();

        return game.PlayerCards;
    }
}
