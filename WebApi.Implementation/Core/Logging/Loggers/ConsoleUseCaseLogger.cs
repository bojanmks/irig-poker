using Newtonsoft.Json;
using WebApi.Application.Core.Logging;
using WebApi.Application.Core.Logging.LoggerData;

namespace WebApi.Implementation.Core.Logging.Loggers
{
    public class ConsoleUseCaseLogger : IUseCaseLogger
    {
        public Task Log(UseCaseLoggerData data)
        {
            Console.WriteLine(JsonConvert.SerializeObject(data));
            return Task.CompletedTask;
        }
    }
}
