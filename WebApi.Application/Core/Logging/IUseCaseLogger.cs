using WebApi.Application.Core.Logging.LoggerData;

namespace WebApi.Application.Core.Logging
{
    public interface IUseCaseLogger
    {
        Task Log(UseCaseLoggerData data);
    }
}
