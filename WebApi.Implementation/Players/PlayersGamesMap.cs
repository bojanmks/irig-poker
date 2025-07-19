using System.Collections.Concurrent;

namespace WebApi.Implementation.Players;

public class PlayersGamesMap
{
    public ConcurrentDictionary<string, string> Map { get; } = new();
}
