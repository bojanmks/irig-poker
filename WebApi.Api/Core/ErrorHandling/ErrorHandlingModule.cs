

using WebApi.Api.Core.ErrorHandling.Middleware;
using WebApi.Api.Core.Modules;

namespace WebApi.Api.Core.ErrorHandling;

public class ErrorHandlingModule : BaseModule
{
    public override void UseServices(WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
