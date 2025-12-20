using System.Globalization;

namespace WebApi.Application.Core.Localization;

public interface ILocaleResolver
{
    CultureInfo Resolve();
    void ForceLocale(CultureInfo cultureInfo);
}
