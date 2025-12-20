using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Joining.Models;

public class PlayerJoinNotification
{
    public string ConnectionId { get; set; }
    public PlayerDto Player { get; set; }
}
