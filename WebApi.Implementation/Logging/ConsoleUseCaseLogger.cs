using Newtonsoft.Json;
using WebApi.Application.Logging;
using WebApi.Application.Logging.LoggerData;

namespace WebApi.Implementation.Logging
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
