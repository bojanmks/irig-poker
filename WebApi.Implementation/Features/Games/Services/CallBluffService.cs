using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.Services;

public class CallBluffService(
    IGetGameService _getGameService
) : ICallBluffService
{
    public async Task<CallBluffServiceResult> CallBluffAsync(string gameCode, string callingPlayerId, CancellationToken cancellationToken = default)
    {
        var game = await _getGameService.GetAsync(gameCode, cancellationToken) ?? throw new InvalidOperationException("Game not found");

        var claimingPlayerId = game.ClaimingPlayerId!;
        var claimedHand = game.CurrentClaimedHand!.Value;
        var ranks = game.Ranks!;
        var claimedSuit = game.ClaimedSuit;

        var allCards = game.GetAllCombinedCards();
        var wasTruthful = HandEvaluator.HandExistsWithRanks(allCards, claimedHand, ranks);

        string losingPlayerId = wasTruthful ? callingPlayerId : claimingPlayerId;

        var allPlayerCardsSnapshot = game.PlayerCards
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());

        game.AddCardToPlayer(losingPlayerId);
        game.DealCardsToAllPlayers();
        var loserNewCardCount = game.Players[losingPlayerId].CardCount;

        string? eliminatedPlayerId = null;
        string? winnerPlayerId = null;
        string? winnerUsername = null;

        if (loserNewCardCount >= game.MaxCardCount)
        {
            eliminatedPlayerId = losingPlayerId;
            game.RemovePlayer(losingPlayerId);

            if (game.ActivePlayerIds.Count <= 1)
            {
                var lastPlayerId = game.ActivePlayerIds.FirstOrDefault();
                if (lastPlayerId is not null)
                {
                    winnerPlayerId = lastPlayerId;
                    winnerUsername = game.Players[lastPlayerId].Username;
                }
            }
        }

        game.ClearClaim();

        PublicGameState? updatedGameState = null;

        if (winnerPlayerId is null)
        {
            game.StartNewRound();

            updatedGameState = new PublicGameState(
                game.GameCode,
                game.HasStarted,
                game.Players,
                game.PlayerOrder,
                game.CurrentTurnPlayerId,
                null,
                null,
                null,
                null,
                game.MaxCardCount
            );
        }

        var resolution = new RoundResolvedNotification(
            claimingPlayerId,
            claimedHand,
            ranks,
            claimedSuit,
            callingPlayerId,
            wasTruthful,
            losingPlayerId,
            loserNewCardCount,
            allPlayerCardsSnapshot,
            eliminatedPlayerId,
            winnerPlayerId,
            winnerUsername
        );

        var playerCardsCopy = game.PlayerCards
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());

        return new CallBluffServiceResult(
            resolution,
            eliminatedPlayerId,
            winnerPlayerId,
            winnerUsername,
            updatedGameState,
            playerCardsCopy
        );
    }
}
