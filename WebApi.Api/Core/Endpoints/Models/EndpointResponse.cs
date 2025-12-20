using WebApi.Common.Core.Result.Models;

namespace WebApi.Api.Core.Endpoints.Models
{
    public class EndpointResponse<T>
    {
        public T Data { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<FieldErrors> FieldErrors { get; set; } = Enumerable.Empty<FieldErrors>();
        public int StatusCode { get; set; }
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
    }
}
