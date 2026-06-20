using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Players.Models;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Implementation.Features.Games.Services;

public class AddPlayerToGameService(
    IGetGameService _getGameService,
    PlayersGamesMap _playersGamesMap,
    IHubConnectionIdProvider _hubConnectionIdProvider
) : IAddPlayerToGameService
{
    public async Task<bool> AddAsync(JoinGameDto data, CancellationToken cancellationToken = default)
    {
        if (!_hubConnectionIdProvider.TryGetConnectionId(out var connectionId))
        {
            return false;
        }

        var game = await _getGameService.GetAsync(data.GameCode, cancellationToken);

        if (game is null)
        {
            return false;
        }

        var player = new PlayerDto
        {
            Username = data.Username
        };

        if (!game.Players.Any())
        {
            player.SetIsAdmin(true);
        }

        bool addToGameResult = game.Players.TryAdd(connectionId, player);

        if (!addToGameResult)
        {
            return false;
        }

        bool addToMapResult = _playersGamesMap.Map.TryAdd(connectionId, data.GameCode);

        if (!addToMapResult)
        {
            game!.Players.Remove(connectionId, out _);
            return false;
        }

        return true;
    }
}
