using WebApi.Common.Features.Games.Models;

namespace WebApi.Common.Features.Games.Joining.Models;

public record JoinGameResultDto(string PlayerId, PublicGameStateDto GameState);
