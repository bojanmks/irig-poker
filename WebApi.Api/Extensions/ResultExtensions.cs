using WebApi.Api.Hubs;
using WebApi.Common.DTO.Result;

namespace WebApi.Api.Extensions;

public static class ResultExtensions
{
    public static HubActionResponse<T> ToHubActionResponse<T>(this Result<T> result)
    {
        return new HubActionResponse<T>
        {
            Data = result.Data,
            Errors = result.Errors,
            FieldErrors = result.FieldErrors,
        };
    }
}
