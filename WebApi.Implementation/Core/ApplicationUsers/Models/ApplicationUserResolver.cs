using System.Globalization;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Services;
using WebApi.Common.Core.Auth.Enums;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Implementation.Core.ApplicationUsers.Models;

public class ApplicationUserResolver(
    ILocaleResolver _localeGetter,
    PlayersGamesMap _playersGamesMap,
    IGetGameService _getGameService
) : IApplicationUserResolver
{
    private string? _connectionId = null;

    private static ApplicationUser GetNotPlayingUser(string? connectionId, CultureInfo locale)
    {
        return new ApplicationUser
        {
            Locale = locale,
            Role = UserRole.NotPlaying,
            ConnectionId = connectionId,
            GameCode = null
        };
    }

    public async Task<IApplicationUser> ResolveAsync(CancellationToken cancellationToken = default)
    {
        var locale = _localeGetter.Resolve();

        if (string.IsNullOrWhiteSpace(_connectionId) || !_playersGamesMap.Map.TryGetValue(_connectionId, out string? gameCode))
        {
            return GetNotPlayingUser(_connectionId, locale);
        }

        var game = await _getGameService.GetAsync(gameCode, cancellationToken);

        if (game is null)
        {
            return GetNotPlayingUser(_connectionId, locale);
        }

        var userRole = game.Players[_connectionId].IsAdmin ? UserRole.RoomOwner : UserRole.Player;

        return new ApplicationUser
        {
            Locale = locale,
            ConnectionId = _connectionId,
            GameCode = gameCode,
            Role = userRole
        };
    }

    public void SetConnectionId(string connectionId)
    {
        _connectionId = connectionId;
    }
}
