using WebApi.Common.Core.Localization.Contracts;

namespace WebApi.Api.Core.Hubs.Models;

public class HubActionRequest<T> : IHasLocaleInfo
{
    public T Data { get; set; }
    public string LanguageCode { get; set; }
}
