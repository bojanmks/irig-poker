
using FastEndpoints;
using WebApi.Api.Core.Modules;

namespace WebApi.Api.Core.Endpoints;

public class EndpointsModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddFastEndpoints();
    }

    public override void UseServices(WebApplication app)
    {
        app.UseFastEndpoints(config =>
        {
            config.Endpoints.AllowEmptyRequestDtos = true;
            config.Endpoints.RoutePrefix = "api";
        });
    }
}
