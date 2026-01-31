using WebApi.Api.Core;
using WebApi.Api.Features.Games.Hubs;
using WebApi.Application.Features.Games.Services;
using WebApi.Implementation.Features.Games.Services;
using WebApi.Implementation.Features.Games.Stores;

namespace WebApi.Api.Features.Games;

public class GamesModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<GameStore>();
        services.AddSingleton<PlayersGamesMap>();
        services.AddTransient<ICreateGameService, CreateGameService>();
        services.AddTransient<IGetGameService, GetGameService>();
        services.AddTransient<IDeleteGameService, DeleteGameService>();
        services.AddTransient<IGameExistsService, GameExistsService>();
        services.AddTransient<IAddPlayerToGameService, AddPlayerToGameService>();
        services.AddTransient<IDisconnectFromGameService, DisconnectFromGameService>();
        services.AddTransient<IStartGameService, StartGameService>();
    }

    public override void UseServices(WebApplication app)
    {
        app.MapHub<GameHub>("/hubs/game");
    }
}
