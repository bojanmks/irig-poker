using Microsoft.AspNetCore.Http;
using System.Globalization;
using WebApi.Application.Core.Localization;
using WebApi.Implementation.Core.Localization.Constants;

namespace WebApi.Implementation.Core.Localization.Resolvers;

public class LocaleResolver : ILocaleResolver
{
    private readonly IHttpContextAccessor _accessor;
    private CultureInfo? _forcedLocale = null;

    public LocaleResolver(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public CultureInfo Resolve()
    {
        if (_forcedLocale is not null)
        {
            return _forcedLocale;
        }

        if (_accessor == null)
        {
            return DefaultLocale;
        }

        if (_accessor.HttpContext == null)
        {
            return DefaultLocale;
        }

        var request = _accessor.HttpContext.Request;
        var localeCode = (string?)request.Headers["Accept-Language"];

        if (localeCode is not null && LocaleConstants.SupportedLocales.Contains(localeCode))
        {
            return new CultureInfo(localeCode);
        }

        return DefaultLocale;
    }

    public void ForceLocale(CultureInfo cultureInfo)
    {
        _forcedLocale = cultureInfo;
    }

    private CultureInfo DefaultLocale => new CultureInfo(LocaleConstants.DefaultLocale);
}
