

using WebApi.Api.Core.ErrorHandling.Middleware;

namespace WebApi.Api.Core.ErrorHandling;

public class ErrorHandlingModule : BaseModule
{
    public override void UseServices(WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
