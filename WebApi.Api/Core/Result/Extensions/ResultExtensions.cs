using WebApi.Api.Core.Hubs.Models;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Api.Core.Result.Extensions;

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
