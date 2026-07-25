
using IrigPoker.Api.Core.Modules;
using IrigPoker.Application.Core.Logging;
using IrigPoker.Implementation.Core.Logging.Loggers;

namespace IrigPoker.Api.Core.Logging;

public class LoggingModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IExceptionLogger, ConsoleExceptionLogger>();
    }
}
