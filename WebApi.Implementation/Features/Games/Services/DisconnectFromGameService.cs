using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Disconnecting.Models;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Implementation.Features.Games.Services;

public class DisconnectFromGameService(
    PlayersGamesMap _playersGamesMap,
    IGetGameService _getGameService,
    IDeleteGameService _deleteGameService
) : IDisconnectFromGameService
{
    public async Task<DisconnectResultDto> DisconnectAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        var result = new DisconnectResultDto();

        if (!_playersGamesMap.Map.TryRemove(connectionId, out string? gameCode))
        {
            return result;
        }

        var game = await _getGameService.GetAsync(gameCode, cancellationToken);

        if (game is null)
        {
            return result;
        }

        result.GameCode = game.GameCode;

        if (!game.Players.TryGetValue(connectionId, out var player))
        {
            return result;
        }

        game.Players.Remove(connectionId, out _);

        if (game.HasStarted && game.Players.Count() <= 1)
        {
            await _deleteGameService.DeleteAsync(gameCode, cancellationToken);
            result.HasGameEnded = true;
            return result;
        }

        if (!player.IsAdmin)
        {
            return result;
        }

        player.IsAdmin = false;
        var newAdmin = game.Players.FirstOrDefault(x => x.Key != connectionId);

        if (newAdmin.Value is null)
        {
            await _deleteGameService.DeleteAsync(gameCode, cancellationToken);
            result.HasGameEnded = true;
            return result;
        }

        newAdmin.Value.IsAdmin = true;
        result.ChangedAdminTo = newAdmin.Key;

        return result;
    }
}
