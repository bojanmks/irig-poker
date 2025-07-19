using System.Collections.Concurrent;
using WebApi.Common.DTO.Games;

namespace WebApi.Implementation.Games;

public class GameStore
{
    public ConcurrentDictionary<string, GameDto> GameStates { get; } = new();
}