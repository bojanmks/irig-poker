

using WebApi.Api.Core.Modules;

namespace WebApi.Api.Core.Cors;

public class CorsModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173", "https://irigpoker.bojanm.dev")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    public override void UseServices(WebApplication app)
    {
        app.UseCors("AllowSpecificOrigins");
    }
}
