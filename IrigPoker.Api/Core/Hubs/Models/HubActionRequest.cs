using IrigPoker.Common.Core.Localization.Contracts;

namespace IrigPoker.Api.Core.Hubs.Models;

public record HubActionRequest<T> : IHasLocaleInfo
{
    public required T Data { get; init; }
    public required string LanguageCode { get; init; }
}
