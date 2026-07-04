using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;
using WebApi.Common.Features.Players.Models;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Implementation.Features.Games.Services;

public class AddPlayerToGameService(
    IGetGameService _getGameService,
    PlayersGamesMap _playersGamesMap,
    IHubConnectionIdProvider _hubConnectionIdProvider
) : IAddPlayerToGameService
{
    public async Task<(string? PlayerId, GameState? GameState)> AddAsync(JoinGameRequest data, CancellationToken cancellationToken = default)
    {
        if (!_hubConnectionIdProvider.TryGetConnectionId(out var connectionId))
        {
            return (null, null);
        }

        var game = await _getGameService.GetAsync(data.GameCode, cancellationToken);

        if (game is null)
        {
            return (null, null);
        }

        var playerId = Guid.NewGuid().ToString("N");

        var player = new Player
        {
            PlayerId = playerId,
            Username = data.Username
        };

        if (!game.Players.Any())
        {
            player.SetIsAdmin(true);
        }

        bool addToGameResult = game.Players.TryAdd(playerId, player);

        if (!addToGameResult)
        {
            return (null, null);
        }

        bool addToMapResult = _playersGamesMap.Map.TryAdd(connectionId, (playerId, data.GameCode));

        if (!addToMapResult)
        {
            game.Players.Remove(playerId, out _);
            return (null, null);
        }

        _playersGamesMap.PlayerIdToConnectionId[playerId] = connectionId;

        game.PlayerOrder.Add(playerId);
        game.ActivePlayerIds.Add(playerId);

        game.UpdateCardCountThreshold();

        game.LastActivityAt = DateTimeOffset.UtcNow;

        return (playerId, game);
    }
}
