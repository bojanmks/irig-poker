using System.Globalization;
using Microsoft.AspNetCore.Http;
using WebApi.Application.Localization;

namespace WebApi.Implementation.Localization;

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
        var localeCode = request.Headers["Accept-Language"];

        if (LocaleConstants.SupportedLocales.Contains(localeCode))
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
