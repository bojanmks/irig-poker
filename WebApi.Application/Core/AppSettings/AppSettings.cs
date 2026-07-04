namespace WebApi.Application.Core.AppSettings;

public class AppSettings
{
    public int MinPlayersPerGame { get; init; }

    public int StaleGameTimeoutMinutes { get; init; }
    public int GameCleanupIntervalMinutes { get; init; }
}
