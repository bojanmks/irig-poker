using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Implementation.Features.Games.Stores;

public class PlayersGamesMap
{
    public ConcurrentDictionary<string, (string PlayerId, string GameCode)> Map { get; } = new();
    public ConcurrentDictionary<string, string> PlayerIdToConnectionId { get; } = new();

    public bool TryGetConnectionId(string playerId, [NotNullWhen(true)] out string? connectionId)
    {
        if (PlayerIdToConnectionId.TryGetValue(playerId, out var id))
        {
            connectionId = id;
            return true;
        }

        connectionId = null;
        return false;
    }
}
