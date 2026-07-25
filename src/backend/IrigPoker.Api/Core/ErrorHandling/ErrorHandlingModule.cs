

using IrigPoker.Api.Core.ErrorHandling.Middleware;
using IrigPoker.Api.Core.Modules;

namespace IrigPoker.Api.Core.ErrorHandling;

public class ErrorHandlingModule : BaseModule
{
    public override void UseServices(WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
