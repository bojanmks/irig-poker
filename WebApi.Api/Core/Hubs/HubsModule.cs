using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Filters;
using WebApi.Api.Core.Hubs.Registries;
using WebApi.Api.Core.Modules;

namespace WebApi.Api.Core.Hubs;

public class HubsModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IHubCallerContextSetter, HubCallerContextRegistry>();
        services.AddSingleton<IHubCallerContextAccessor, HubCallerContextRegistry>();

        services.AddSignalR(options =>
        {
            options.AddFilter<GlobalHubFilter>();
        });
    }
}
