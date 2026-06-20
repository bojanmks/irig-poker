using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Joining.Models;

public record PlayerJoinNotification(string PlayerId, PlayerDto Player);