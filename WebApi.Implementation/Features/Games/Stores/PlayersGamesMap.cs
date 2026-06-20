using System.Collections.Concurrent;

namespace WebApi.Implementation.Features.Games.Stores;

public class PlayersGamesMap
{
    public ConcurrentDictionary<string, (string PlayerId, string GameCode)> Map { get; } = new();
}
