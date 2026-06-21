using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Disconnecting.Models;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Implementation.Features.Games.Services;

public class DisconnectFromGameService(
    PlayersGamesMap _playersGamesMap,
    IGetGameService _getGameService,
    IDeleteGameService _deleteGameService,
    IGameLockService _gameLockService
) : IDisconnectFromGameService
{
    public async Task<DisconnectResultDto> DisconnectAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        var result = new DisconnectResultDto();

        if (!_playersGamesMap.Map.TryRemove(connectionId, out var entry))
        {
            return result;
        }

        result.PlayerId = entry.PlayerId;

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

            if (game.HasStarted && game.Players.Count() <= 1)
            {
                await _deleteGameService.DeleteAsync(entry.GameCode, cancellationToken);
                result.HasGameEnded = true;
                return result;
            }

            if (!player.IsAdmin)
            {
                return result;
            }

            player.SetIsAdmin(false);
            var newAdmin = game.Players.FirstOrDefault(x => x.Key != entry.PlayerId);

            if (newAdmin.Value is null)
            {
                await _deleteGameService.DeleteAsync(entry.GameCode, cancellationToken);
                result.HasGameEnded = true;
                return result;
            }

            newAdmin.Value.SetIsAdmin(true);
            result.ChangedAdminTo = newAdmin.Key;

            return result;
        }
    }
}
