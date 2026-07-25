using IrigPoker.Application.Features.Games.Services;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Implementation.Features.Games.Services;

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

        game.LastActivityAt = DateTimeOffset.UtcNow;

        return game.PlayerCards;
    }
}
