using System.Collections.Concurrent;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Implementation.Features.Games.Services;

public class GameStore
{
    public ConcurrentDictionary<string, GameDto> Games { get; } = new();
}