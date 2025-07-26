using WebApi.Application.ApplicationUsers;
using WebApi.Application.Games;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Players;
using WebApi.Implementation.Players;

namespace WebApi.Implementation.Games;

public class AddPlayerToGameService(
    IGetGameService _getGameService,
    PlayersGamesMap _playersGamesMap,
    IApplicationUserResolver _applicationUserResolver
) : IAddPlayerToGameService
{
    public async Task<bool> AddAsync(JoinGameDto data, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        var game = await _getGameService.GetAsync(data.GameCode, cancellationToken);

        bool addToGameResult = game!.Players.TryAdd(applicationUser.ConnectionId, new PlayerDto
        {
            Username = data.Username,
            IsAdmin = !game.Players.Any()
        });

        if (!addToGameResult)
        {
            return false;
        }

        bool addToMapResult = _playersGamesMap.Map.TryAdd(applicationUser.ConnectionId, data.GameCode);

        if (!addToMapResult)
        {
            game!.Players.Remove(applicationUser.ConnectionId, out _);
            return false;
        }

        return true;
    }
}
