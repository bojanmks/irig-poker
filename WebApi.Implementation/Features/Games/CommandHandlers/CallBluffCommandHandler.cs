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
    ICallBluffService _callBluffService,
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

            var serviceResult = await _callBluffService.CallBluffAsync(gameCode, applicationUser.PlayerId!, cancellationToken);

            return new CallBluffResult(
                gameCode,
                serviceResult.Resolution,
                serviceResult.EliminatedPlayerId,
                serviceResult.WinnerPlayerId,
                serviceResult.WinnerUsername,
                serviceResult.UpdatedGameState,
                serviceResult.PlayerCards
            );
        }
    }
}
