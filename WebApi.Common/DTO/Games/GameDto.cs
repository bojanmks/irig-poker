using System.Collections.Concurrent;
using WebApi.Common.DTO.Players;

namespace WebApi.Common.DTO.Games;

public class GameDto
{
    public string GameCode { get; set; }
    public ConcurrentDictionary<string, PlayerDto> Players { get; set; } = new();
}