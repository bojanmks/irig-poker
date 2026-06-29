using MediatR;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

public class CallBluffCommandHandler(
    IApplicationUserResolver _applicationUserResolver,
    IGetGameService _getGameService,
    IGameLockService _gameLockService,
    ITranslator _translator
) : IRequestHandler<CallBluffCommand, Result<CallBluffResult>>
{
    public async Task<Result<CallBluffResult>> Handle(CallBluffCommand command, CancellationToken cancellationToken)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(applicationUser.GameCode))
        {
            return Result<CallBluffResult>.Error(_translator.Translate("user.notInGame"));
        }

        string gameCode = applicationUser.GameCode;

        using (await _gameLockService.AcquireLockAsync(gameCode, cancellationToken))
        {
            var game = await _getGameService.GetAsync(gameCode, cancellationToken);

            if (game is null)
            {
                return Result<CallBluffResult>.Error(_translator.Translate("game.notFound"));
            }

            if (!game.HasStarted)
            {
                return Result<CallBluffResult>.Error(_translator.Translate("game.notStarted"));
            }

            if (game.CurrentTurnPlayerId != applicationUser.PlayerId)
            {
                return Result<CallBluffResult>.Error(_translator.Translate("game.notYourTurn"));
            }

            if (game.ClaimingPlayerId is null || game.CurrentClaimedHand is null || game.Ranks is null)
            {
                return Result<CallBluffResult>.Error(_translator.Translate("game.noClaimToCall"));
            }

            if (game.ClaimingPlayerId == applicationUser.PlayerId)
            {
                return Result<CallBluffResult>.Error(_translator.Translate("game.cannotCallOwnBluff"));
            }

            var claimingPlayerId = game.ClaimingPlayerId;
            var claimedHand = game.CurrentClaimedHand.Value;
            var ranks = game.Ranks;

            var allCards = game.GetAllCombinedCards();
            var wasTruthful = HandEvaluator.HandExistsWithRanks(allCards, claimedHand, ranks);

            string losingPlayerId;
            if (wasTruthful)
            {
                losingPlayerId = applicationUser.PlayerId!;
            }
            else
            {
                losingPlayerId = claimingPlayerId;
            }

            var allPlayerCardsSnapshot = game.PlayerCards
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());

            game.AddCardToPlayer(losingPlayerId);
            game.DealCardsToAllPlayers();
            var loserNewCardCount = game.Players[losingPlayerId].CardCount;

            string? eliminatedPlayerId = null;
            string? winnerPlayerId = null;
            string? winnerUsername = null;

            if (loserNewCardCount >= GameState.MaxCardCount)
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
                game.StartRound();

                updatedGameState = new PublicGameState(
                    game.GameCode,
                    game.HasStarted,
                    game.Players,
                    game.PlayerOrder,
                    game.CurrentTurnPlayerId,
                    null,
                    null,
                    null
                );
            }

            var resolution = new RoundResolvedNotification(
                claimingPlayerId,
                claimedHand,
                ranks,
                applicationUser.PlayerId!,
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

            return new CallBluffResult(
                gameCode,
                resolution,
                eliminatedPlayerId,
                winnerPlayerId,
                winnerUsername,
                updatedGameState,
                playerCardsCopy
            );
        }
    }
}
