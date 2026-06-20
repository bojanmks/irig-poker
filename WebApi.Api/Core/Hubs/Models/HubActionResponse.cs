using System.Diagnostics.CodeAnalysis;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Api.Core.Hubs.Models;

public class HubActionResponse<T>
{
    public T? Data { get; init; }
    public IEnumerable<string>? Errors { get; init; } = [];
    public IEnumerable<FieldErrors>? FieldErrors { get; init; } = [];

    [MemberNotNullWhen(true, nameof(Data))]
    public bool IsSuccess => Errors?.Any() != true && FieldErrors?.Any() != true;
}
