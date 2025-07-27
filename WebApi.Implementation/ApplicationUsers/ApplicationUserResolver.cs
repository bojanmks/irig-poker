using WebApi.Application.ApplicationUsers;
using WebApi.Application.Games;
using WebApi.Application.Localization;
using WebApi.Common.Enums.Auth;
using WebApi.Implementation.Players;
using WebApi.Implementation.UseCases;

namespace WebApi.Implementation.ApplicationUsers;

public class ApplicationUserResolver(
    ILocaleResolver _localeGetter,
    UserRoleUseCaseMapStore _userRoleUseCaseMapStore,
    IUserSessionRegistry _userSessionRegistry,
    PlayersGamesMap _playersGamesMap,
    IGetGameService _getGameService
) : IApplicationUserResolver
{
    public async Task<IApplicationUser> ResolveAsync(CancellationToken cancellationToken = default)
    {
        var locale = _localeGetter.Resolve();

        var notPlayingUser = new ApplicationUser
        {
            Locale = locale,
            Role = UserRole.NotPlaying,
            AllowedUseCases = _userRoleUseCaseMapStore.GetUseCases(UserRole.NotPlaying)
        };

        string connectionId = _userSessionRegistry.ConnectionId;
        notPlayingUser.ConnectionId = connectionId;

        if (string.IsNullOrWhiteSpace(connectionId) || !_playersGamesMap.Map.TryGetValue(connectionId, out string? gameCode))
        {
            return notPlayingUser;
        }

        var game = await _getGameService.GetAsync(gameCode, cancellationToken);

        if (game is null)
        {
            return notPlayingUser;
        }

        var userRole = game.Players[connectionId].IsAdmin ? UserRole.RoomOwner : UserRole.Player;

        return new ApplicationUser
        {
            Locale = locale,
            ConnectionId = connectionId,
            GameCode = gameCode,
            Role = userRole,
            AllowedUseCases = _userRoleUseCaseMapStore.GetUseCases(userRole)
        };
    }
}
