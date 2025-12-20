using System.Collections.Concurrent;

namespace WebApi.Implementation.Features.Games.Stores;

public class PlayersGamesMap
{
    public ConcurrentDictionary<string, string> Map { get; } = new();
}
