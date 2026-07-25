using System.Globalization;

namespace IrigPoker.Application.Core.Localization;

public interface ILocaleResolver
{
    CultureInfo Resolve();
    void ForceLocale(CultureInfo cultureInfo);
}
