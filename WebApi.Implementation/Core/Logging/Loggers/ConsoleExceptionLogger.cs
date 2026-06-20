using WebApi.Application.Core.Logging;

namespace WebApi.Implementation.Core.Logging.Loggers;

public class ConsoleExceptionLogger : IExceptionLogger
{
    public Task Log(Exception ex)
    {
        Console.WriteLine(ex);
        return Task.CompletedTask;
    }
}
