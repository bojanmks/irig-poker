namespace WebApi.Application.Core.Logging;

public interface IExceptionLogger
{
    Task Log(Exception ex);
}
