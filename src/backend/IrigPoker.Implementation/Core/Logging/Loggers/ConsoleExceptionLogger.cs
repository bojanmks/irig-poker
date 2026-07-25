using IrigPoker.Application.Core.Logging;

namespace IrigPoker.Implementation.Core.Logging.Loggers;

public class ConsoleExceptionLogger : IExceptionLogger
{
    public Task Log(Exception ex)
    {
        Console.WriteLine(ex);
        return Task.CompletedTask;
    }
}
