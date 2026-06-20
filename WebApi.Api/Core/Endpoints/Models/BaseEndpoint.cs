using FastEndpoints;
using System.Net;
using WebApi.Common.Core.Result.Enums;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Api.Core.Endpoints.Models;

public abstract class BaseEndpoint<TRequest, TResponse> : Endpoint<TRequest, EndpointResponse<TResponse>>
    where TRequest : notnull
{
    public override void Configure()
    {
        AllowAnonymous();
        ConfigureEndpoint();
    }

    protected abstract void ConfigureEndpoint();

    protected async Task RespondFromResult(Result<TResponse> result, CancellationToken cancellationToken = default)
    {
        int statusCode = result.HttpStatusCode.HasValue
            ? result.HttpStatusCode.Value
            : result.Status switch
            {
                ResultStatus.Success => (int)HttpStatusCode.OK,
                ResultStatus.Error => (int)HttpStatusCode.InternalServerError,
                ResultStatus.ValidationError => (int)HttpStatusCode.UnprocessableEntity,
                ResultStatus.NotFound => (int)HttpStatusCode.NotFound,
                _ => throw new ArgumentOutOfRangeException(nameof(result.Status), $"Unexpected result status value: {result.Status}")
            };

        var endpointResponse = new EndpointResponse<TResponse>
        {
            Data = result.Data,
            ErrorMessages = result.Errors ?? [],
            FieldErrors = result.FieldErrors ?? [],
            StatusCode = statusCode
        };

        await Send.ResponseAsync(
            endpointResponse,
            statusCode: statusCode,
            cancellation: cancellationToken
        );
    }
}