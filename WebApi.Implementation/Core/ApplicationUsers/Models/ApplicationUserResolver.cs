using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Auth.Enums;
using WebApi.Implementation.Core.UseCases.Stores;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Implementation.Core.ApplicationUsers.Models;

public class ApplicationUserResolver(
    ILocaleResolver _localeGetter,
    UserRoleUseCaseMapStore _userRoleUseCaseMapStore,
    PlayersGamesMap _playersGamesMap,
    IGetGameService _getGameService
) : IApplicationUserResolver
{
    private string? _connectionId = null;

    public async Task<IApplicationUser> ResolveAsync(CancellationToken cancellationToken = default)
    {
        var locale = _localeGetter.Resolve();

        var notPlayingUser = new ApplicationUser
        {
            Locale = locale,
            Role = UserRole.NotPlaying,
            AllowedUseCases = _userRoleUseCaseMapStore.GetUseCases(UserRole.NotPlaying),
            ConnectionId = _connectionId
        };

        if (string.IsNullOrWhiteSpace(_connectionId) || !_playersGamesMap.Map.TryGetValue(_connectionId, out string? gameCode))
        {
            return notPlayingUser;
        }

        var game = await _getGameService.GetAsync(gameCode, cancellationToken);

        if (game is null)
        {
            return notPlayingUser;
        }

        var userRole = game.Players[_connectionId].IsAdmin ? UserRole.RoomOwner : UserRole.Player;

        return new ApplicationUser
        {
            Locale = locale,
            ConnectionId = _connectionId,
            GameCode = gameCode,
            Role = userRole,
            AllowedUseCases = _userRoleUseCaseMapStore.GetUseCases(userRole)
        };
    }

    public void SetConnectionId(string connectionId)
    {
        _connectionId = connectionId;
    }
}
