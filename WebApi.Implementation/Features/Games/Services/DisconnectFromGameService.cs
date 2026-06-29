using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Disconnecting.Models;
using WebApi.Common.Features.Games.Models;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Implementation.Features.Games.Services;

public class DisconnectFromGameService(
    PlayersGamesMap _playersGamesMap,
    IGetGameService _getGameService,
    IDeleteGameService _deleteGameService,
    IGameLockService _gameLockService
) : IDisconnectFromGameService
{
    public async Task<DisconnectResult> DisconnectAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        var result = new DisconnectResult();

        if (!_playersGamesMap.Map.TryRemove(connectionId, out var entry))
        {
            return result;
        }

        result.PlayerId = entry.PlayerId;
        _playersGamesMap.PlayerIdToConnectionId.TryRemove(entry.PlayerId, out _);

        using (await _gameLockService.AcquireLockAsync(entry.GameCode, cancellationToken))
        {
            var game = await _getGameService.GetAsync(entry.GameCode, cancellationToken);

            if (game is null)
            {
                return result;
            }

            result.GameCode = game.GameCode;

            if (!game.Players.TryGetValue(entry.PlayerId, out var player))
            {
                return result;
            }

            game.Players.Remove(entry.PlayerId, out _);
            game.PlayerOrder.Remove(entry.PlayerId);
            game.ActivePlayerIds.Remove(entry.PlayerId);

            if (game.HasStarted && game.ActivePlayerIds.Count <= 1)
            {
                if (game.ActivePlayerIds.Count == 1)
                {
                    var winnerPlayerId = game.ActivePlayerIds.First();
                    var winner = game.Players[winnerPlayerId];
                    result.WinnerPlayerId = winner.PlayerId;
                    result.WinnerUsername = winner.Username;
                }

                await _deleteGameService.DeleteAsync(entry.GameCode, cancellationToken);
                result.HasGameEnded = true;
                return result;
            }

            if (game.HasStarted)
            {
                game.StartNewRound();
            }

            if (!player.IsAdmin)
            {
                result.UpdatedGameState = new PublicGameState(
                    game.GameCode,
                    game.HasStarted,
                    game.Players,
                    game.PlayerOrder,
                    game.CurrentTurnPlayerId,
                    game.CurrentClaimedHand,
                    game.ClaimingPlayerId,
                    game.Ranks,
                    game.ClaimedSuit
                );
                return result;
            }

            player.SetIsAdmin(false);
            var newAdmin = game.Players.First(x => x.Key != entry.PlayerId);

            newAdmin.Value.SetIsAdmin(true);
            result.ChangedAdminTo = newAdmin.Key;

            result.UpdatedGameState = new PublicGameState(
                game.GameCode,
                game.HasStarted,
                game.Players,
                game.PlayerOrder,
                game.CurrentTurnPlayerId,
                game.CurrentClaimedHand,
                game.ClaimingPlayerId,
                game.Ranks,
                game.ClaimedSuit
            );

            return result;
        }
    }
}
