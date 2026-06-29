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
    IGetGameService _getGameService,
    IHubConnectionIdProvider _hubConnectionIdProvider
) : IApplicationUserResolver
{
    private static ApplicationUser GetNotPlayingUser(CultureInfo locale)
    {
        return new ApplicationUser
        {
            Locale = locale,
            Role = UserRole.NotPlaying,
            GameCode = null,
            PlayerId = null
        };
    }

    public async Task<IApplicationUser> ResolveAsync(CancellationToken cancellationToken = default)
    {
        var locale = _localeGetter.Resolve();

        if (!_hubConnectionIdProvider.TryGetConnectionId(out var connectionId) || !_playersGamesMap.Map.TryGetValue(connectionId, out var entry))
        {
            return GetNotPlayingUser(locale);
        }

        var game = await _getGameService.GetAsync(entry.GameCode, cancellationToken);

        if (game is null)
        {
            return GetNotPlayingUser(locale);
        }

        var userRole = game.Players[entry.PlayerId].IsAdmin ? UserRole.RoomOwner : UserRole.Player;

        return new ApplicationUser
        {
            Locale = locale,
            GameCode = entry.GameCode,
            Role = userRole,
            PlayerId = entry.PlayerId
        };
    }
}
