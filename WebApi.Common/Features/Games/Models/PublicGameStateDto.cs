using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Models;

public record PublicGameStateDto(bool HasStarted, IReadOnlyDictionary<string, PlayerDto> Players);