using System.Collections.Concurrent;
using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Models;

public class GameDto
{
    public string GameCode { get; set; }
    public bool HasStarted { get; set; }
    public ConcurrentDictionary<string, PlayerDto> Players { get; set; } = new();
}