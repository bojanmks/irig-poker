
using WebApi.Api.Core.Modules;
using WebApi.Application.Core.Localization;
using WebApi.Implementation.Core.Localization.Resolvers;
using WebApi.Implementation.Core.Localization.Translators;

namespace WebApi.Api.Core.Localization;

public class LocalizationModule : BaseModule
{
    public override int Priority => 0;
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ILocaleResolver, LocaleResolver>();
        services.AddScoped<ITranslator, JsonTranslator>();
    }
}
