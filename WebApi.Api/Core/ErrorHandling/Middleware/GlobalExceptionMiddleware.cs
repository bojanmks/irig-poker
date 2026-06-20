using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using WebApi.Api.Core.Endpoints.Models;
using WebApi.Application.Core.Localization;
using WebApi.Application.Core.Logging;
using WebApi.Common.Core.Cqrs;

namespace WebApi.Api.Core.ErrorHandling.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception ex)
    {
        var exceptionLogger = httpContext.RequestServices.GetRequiredService<IExceptionLogger>();
        await exceptionLogger.Log(ex);

        var translator = httpContext.RequestServices.GetRequiredService<ITranslator>();

        int statusCode = (int)HttpStatusCode.InternalServerError;

        var responseBody = new EndpointResponse<Empty>
        {
            Data = Empty.Value,
            ErrorMessages = [translator.Translate("common.anErrorOccurred")],
            StatusCode = statusCode
        };

        var serializedBody = JsonConvert.SerializeObject(
            responseBody,
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        );

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(serializedBody);
    }
}