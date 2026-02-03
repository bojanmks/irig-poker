namespace WebApi.Common.Core.Result.Models;

public class FieldErrors
{
    public string Field { get; set; }
    public IEnumerable<string> Errors { get; set; }
}
