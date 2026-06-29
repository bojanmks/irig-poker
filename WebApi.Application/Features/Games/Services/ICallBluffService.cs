using WebApi.Common.Features.Games.Models;

namespace WebApi.Application.Features.Games.Services;

public record CallBluffServiceResult(
    RoundResolvedNotification Resolution,
    string? EliminatedPlayerId,
    string? WinnerPlayerId,
    string? WinnerUsername,
    PublicGameState? UpdatedGameState,
    IReadOnlyDictionary<string, List<Card>> PlayerCards
);

public interface ICallBluffService
{
    Task<CallBluffServiceResult> CallBluffAsync(string gameCode, string callingPlayerId, CancellationToken cancellationToken = default);
}
