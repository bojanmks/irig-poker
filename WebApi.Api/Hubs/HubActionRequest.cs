using WebApi.Common.DTO;

namespace WebApi.Api.Hubs;

public class HubActionRequest<T> : IHasLocaleInfo
{
    public T Data { get; set; }
    public string LanguageCode { get; set; }
}
