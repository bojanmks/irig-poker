using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Core.Cqrs.Pipeline;

public class RequestLoggingBehavior<TRequest, TInner> : IPipelineBehavior<TRequest, Result<TInner>>
    where TRequest : IRequest<Result<TInner>>
{
    private readonly ILogger<RequestLoggingBehavior<TRequest, TInner>> _logger;

    public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TInner>> logger)
    {
        _logger = logger;
    }

    public async Task<Result<TInner>> Handle(TRequest request, RequestHandlerDelegate<Result<TInner>> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestData = JsonConvert.SerializeObject(request);

        _logger.LogInformation("Executing {RequestName}: {RequestData}", requestName, requestData);

        var response = await next();

        _logger.LogInformation(
            "Executed {RequestName} - Success: {IsSuccess}, Status: {Status}",
            requestName, response.IsSuccess, response.Status);

        return response;
    }
}
