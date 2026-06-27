namespace WebApi.Common.Features.Games.Models;

public record StartGameResult(
    PublicGameState GameState,
    IReadOnlyDictionary<string, List<Card>> PlayerCards
);
