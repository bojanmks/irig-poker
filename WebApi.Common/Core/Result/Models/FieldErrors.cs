namespace WebApi.Common.Core.Result.Models;

public record FieldErrors
{
    public required string Field { get; init; }
    public required IEnumerable<string> Errors { get; init; }
}
