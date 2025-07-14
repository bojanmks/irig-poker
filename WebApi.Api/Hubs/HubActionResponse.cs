using WebApi.Common.DTO.Result;

namespace WebApi.Api.Hubs;

public class HubActionResponse<T>
{
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<FieldErrors>? FieldErrors { get; set; } = Enumerable.Empty<FieldErrors>();
    public bool IsSuccess => Errors?.Any() != true && FieldErrors?.Any() != true;
}
