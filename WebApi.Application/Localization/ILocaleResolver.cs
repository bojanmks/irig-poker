using System.Globalization;

namespace WebApi.Application.Localization;

public interface ILocaleResolver
{
    CultureInfo Resolve();
    void ForceLocale(CultureInfo cultureInfo);
}
