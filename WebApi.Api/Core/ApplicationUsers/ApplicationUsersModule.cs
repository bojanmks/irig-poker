
using WebApi.Api.Core.Modules;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Implementation.Core.ApplicationUsers.Models;

namespace WebApi.Api.Core.ApplicationUsers;

public class ApplicationUsersModule : BaseModule
{
    public override int Priority => 1;
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IApplicationUserResolver, ApplicationUserResolver>();
    }
}
