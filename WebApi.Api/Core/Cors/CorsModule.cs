

using WebApi.Api.Core.Modules;

namespace WebApi.Api.Core.Cors;

public class CorsModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    public override void UseServices(WebApplication app)
    {
        app.UseCors("AllowAll");
    }
}
