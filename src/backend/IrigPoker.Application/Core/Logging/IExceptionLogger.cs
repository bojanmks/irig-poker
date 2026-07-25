namespace IrigPoker.Application.Core.Logging;

public interface IExceptionLogger
{
    Task Log(Exception ex);
}
