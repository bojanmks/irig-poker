using WebApi.Common.DTO.Players;

namespace WebApi.Common.DTO.Games;

public class PublicGameState
{
    public Dictionary<string, PlayerDto> Players { get; set; } = new();
}