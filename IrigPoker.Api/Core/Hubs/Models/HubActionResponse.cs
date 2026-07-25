using System.Diagnostics.CodeAnalysis;
using IrigPoker.Common.Core.Result.Models;

namespace IrigPoker.Api.Core.Hubs.Models;

public class HubActionResponse<T>
{
    public T? Data { get; init; }
    public IEnumerable<string>? Errors { get; init; } = [];
    public IEnumerable<FieldErrors>? FieldErrors { get; init; } = [];

    [MemberNotNullWhen(true, nameof(Data))]
    public bool IsSuccess => Errors?.Any() != true && FieldErrors?.Any() != true;
}
