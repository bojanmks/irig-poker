using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Filters;
using WebApi.Api.Core.Hubs.Registries;
using WebApi.Api.Core.Modules;

namespace WebApi.Api.Core.Hubs;

public class HubsModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<HubCallerContextRegistry>();

        services.AddSignalR(options =>
        {
            options.AddFilter<GlobalHubFilter>();
        });
    }
}
