
using WebApi.Api.Core.Modules;
using WebApi.Application.Core.Logging;
using WebApi.Implementation.Core.Logging.Loggers;

namespace WebApi.Api.Core.Logging;

public class LoggingModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IExceptionLogger, ConsoleExceptionLogger>();
    }
}
