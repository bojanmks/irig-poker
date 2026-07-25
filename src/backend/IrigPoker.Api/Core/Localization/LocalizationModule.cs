
using IrigPoker.Api.Core.Modules;
using IrigPoker.Application.Core.Localization;
using IrigPoker.Implementation.Core.Localization.Resolvers;
using IrigPoker.Implementation.Core.Localization.Translators;

namespace IrigPoker.Api.Core.Localization;

public class LocalizationModule : BaseModule
{
    public override int Priority => 0;
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ILocaleResolver, LocaleResolver>();
        services.AddScoped<ITranslator, JsonTranslator>();
    }
}
