using FluentValidation;
using WebApi.Api.Core.Modules;
using WebApi.Application;
using WebApi.Implementation;
using WebApi.Implementation.Core.Cqrs.Pipeline;

namespace WebApi.Api.Core.Cqrs;

public class CqrsModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ImplementationAssemblyMarker).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);

            cfg.AddOpenBehavior(typeof(RequestLoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestAuthorizationBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
        });

        AddValidators(services);
    }

    private static void AddValidators(IServiceCollection services)
    {
        var validatorTypes = typeof(ImplementationAssemblyMarker).Assembly
            .GetTypes()
            .Where(p => p.IsClass && !p.IsAbstract && !p.IsGenericTypeDefinition)
            .Where(p => p.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
            .ToArray();

        foreach (var type in validatorTypes)
        {
            foreach (var iface in type.GetInterfaces().Where(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
            {
                services.AddScoped(iface, type);
            }
        }
    }
}
