using WebApi.Application.Games;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Players;

namespace WebApi.Implementation.Games;

public class AddPlayerToGameService(
    IGetGameService _getGameService    
) : IAddPlayerToGameService
{
    public async Task<bool> AddAsync(JoinGameDto data, CancellationToken cancellationToken = default)
    {
        var game = await _getGameService.GetAsync(data.GameCode);

        bool result = game!.Players.TryAdd(data.ConnectionId, new PlayerDto
        {
            Username = data.Username,
            IsAdmin = !game.Players.Any()
        });

        return result;
    }
}
