using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Filters;
using WebApi.Api.Core.Modules;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Implementation.Core.ApplicationUsers.Models;

namespace WebApi.Api.Core.Hubs;

public class HubsModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<HubConnectionIdProvider>();
        services.AddScoped<IHubConnectionIdProvider>(sp => sp.GetRequiredService<HubConnectionIdProvider>());
        services.AddScoped<IHubConnectionIdSetter>(sp => sp.GetRequiredService<HubConnectionIdProvider>());
        services.AddSignalR(options =>
        {
            options.AddFilter<GlobalHubFilter>();
        });
    }
}
