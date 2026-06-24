using WebApi.Common.Features.Games.Models;

namespace WebApi.Common.Features.Games.Joining.Models;

public record JoinGameResult(string PlayerId, PublicGameState GameState);
