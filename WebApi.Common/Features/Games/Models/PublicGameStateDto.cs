using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Models;

public class PublicGameStateDto
{
    public bool HasStarted { get; set; }
    public Dictionary<string, PlayerDto> Players { get; set; } = new();
}