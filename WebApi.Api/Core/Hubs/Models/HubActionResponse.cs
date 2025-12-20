using WebApi.Common.Core.Result.Models;

namespace WebApi.Api.Core.Hubs.Models;

public class HubActionResponse<T>
{
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<FieldErrors>? FieldErrors { get; set; } = Enumerable.Empty<FieldErrors>();
    public bool IsSuccess => Errors?.Any() != true && FieldErrors?.Any() != true;
}
