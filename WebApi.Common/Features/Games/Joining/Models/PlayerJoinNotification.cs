using WebApi.Common.Features.Players.Models;

namespace WebApi.Common.Features.Games.Joining.Models;

public record PlayerJoinNotification(Player Player, IReadOnlyList<string> PlayerOrder);