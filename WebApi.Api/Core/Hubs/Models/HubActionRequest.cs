using WebApi.Common.Core.Localization.Contracts;

namespace WebApi.Api.Core.Hubs.Models;

public record HubActionRequest<T> : IHasLocaleInfo
{
    public required T Data { get; init; }
    public required string LanguageCode { get; init; }
}
