using System.Diagnostics.CodeAnalysis;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Api.Core.Endpoints.Models;

public class EndpointResponse<T>
{
    public T? Data { get; init; }
    public IEnumerable<string> ErrorMessages { get; init; } = [];
    public IEnumerable<FieldErrors> FieldErrors { get; init; } = [];
    public int StatusCode { get; init; }

    [MemberNotNullWhen(true, nameof(Data))]
    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
}
