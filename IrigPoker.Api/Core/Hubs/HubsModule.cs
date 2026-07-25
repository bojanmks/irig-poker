using Microsoft.AspNetCore.SignalR;
using IrigPoker.Api.Core.Hubs.Filters;
using IrigPoker.Api.Core.Modules;
using IrigPoker.Application.Core.ApplicationUsers;
using IrigPoker.Implementation.Core.ApplicationUsers.Models;

namespace IrigPoker.Api.Core.Hubs;

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
