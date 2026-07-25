using System.Collections.Concurrent;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Implementation.Features.Games.Services;

public class GameStore
{
    public ConcurrentDictionary<string, GameState> Games { get; } = new();
}