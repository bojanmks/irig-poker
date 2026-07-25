
using IrigPoker.Api.Core.Modules;
using IrigPoker.Application.Core.ApplicationUsers;
using IrigPoker.Implementation.Core.ApplicationUsers.Models;

namespace IrigPoker.Api.Core.ApplicationUsers;

public class ApplicationUsersModule : BaseModule
{
    public override int Priority => 1;
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IApplicationUserResolver, ApplicationUserResolver>();
    }
}
