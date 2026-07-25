using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Common.Features.Games.Joining.Models;

public record JoinGameResult(string PlayerId, PublicGameState GameState);
