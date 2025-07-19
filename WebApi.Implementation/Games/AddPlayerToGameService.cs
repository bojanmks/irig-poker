using WebApi.Application.Games;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Players;
using WebApi.Implementation.Players;

namespace WebApi.Implementation.Games;

public class AddPlayerToGameService(
    IGetGameService _getGameService,
    PlayersGamesMap _playersGamesMap
) : IAddPlayerToGameService
{
    public async Task<bool> AddAsync(JoinGameDto data, CancellationToken cancellationToken = default)
    {
        var game = await _getGameService.GetAsync(data.GameCode);

        bool addToGameResult = game!.Players.TryAdd(data.ConnectionId, new PlayerDto
        {
            Username = data.Username,
            IsAdmin = !game.Players.Any()
        });

        if (!addToGameResult)
        {
            return false;
        }

        bool addToMapResult = _playersGamesMap.Map.TryAdd(data.ConnectionId, data.GameCode);

        if (!addToMapResult)
        {
            game!.Players.Remove(data.ConnectionId, out _);
            return false;
        }

        return true;
    }
}
