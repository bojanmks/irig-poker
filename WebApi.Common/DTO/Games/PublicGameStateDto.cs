using WebApi.Common.DTO.Players;

namespace WebApi.Common.DTO.Games;

public class PublicGameStateDto
{
    public bool HasStarted { get; set; }
    public Dictionary<string, PlayerDto> Players { get; set; } = new();
}