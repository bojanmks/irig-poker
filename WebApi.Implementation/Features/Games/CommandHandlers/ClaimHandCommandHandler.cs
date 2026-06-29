using MediatR;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Models;

using static WebApi.Common.Features.Games.Helpers.HandStrengthHelper;

namespace WebApi.Implementation.Features.Games.CommandHandlers;

public class ClaimHandCommandHandler(
    IApplicationUserResolver _applicationUserResolver,
    IGetGameService _getGameService,
    IGameLockService _gameLockService,
    ITranslator _translator
) : IRequestHandler<ClaimHandCommand, Result<ClaimResult>>
{
    public async Task<Result<ClaimResult>> Handle(ClaimHandCommand command, CancellationToken cancellationToken)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(applicationUser.GameCode))
        {
            return Result<ClaimResult>.Error(_translator.Translate("user.notInGame"));
        }

        string gameCode = applicationUser.GameCode;

        using (await _gameLockService.AcquireLockAsync(gameCode, cancellationToken))
        {
            var game = await _getGameService.GetAsync(gameCode, cancellationToken);

            if (game is null)
            {
                return Result<ClaimResult>.Error(_translator.Translate("game.notFound"));
            }

            if (!game.HasStarted)
            {
                return Result<ClaimResult>.Error(_translator.Translate("game.notStarted"));
            }

            if (game.CurrentTurnPlayerId != applicationUser.PlayerId)
            {
                return Result<ClaimResult>.Error(_translator.Translate("game.notYourTurn"));
            }

            if (game.CurrentClaimedHand.HasValue && game.ClaimingPlayerId is not null && game.Ranks is not null)
            {
                if (!IsStrongerThan(command.Data.ClaimedHand, command.Data.Ranks, game.CurrentClaimedHand.Value, game.Ranks))
                {
                    return Result<ClaimResult>.Error(_translator.Translate("game.mustClaimStrongerHand"));
                }
            }

            game.SetClaim(applicationUser.PlayerId!, command.Data.ClaimedHand, command.Data.Ranks);
            game.NextTurn();

            var publicGameState = new PublicGameState(
                game.GameCode,
                game.HasStarted,
                game.Players,
                game.PlayerOrder,
                game.CurrentTurnPlayerId,
                game.CurrentClaimedHand,
                game.ClaimingPlayerId,
                game.Ranks
            );

            return new ClaimResult(
                publicGameState,
                new ClaimNotification(applicationUser.PlayerId!, command.Data.ClaimedHand, command.Data.Ranks)
            );
        }
    }
}
