using WebApi.Common.DTO.Players;

namespace WebApi.Common.DTO.Games;

public class PlayerJoinNotification
{
    public string ConnectionId { get; set; }
    public PlayerDto Player { get; set; }
}
