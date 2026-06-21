using FluentValidation;
using System.Reflection;
using WebApi.Api.Core.Modules;
using WebApi.Application;
using WebApi.Application.Core.Cqrs;
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

        services.AddSingleton<IRequestAuthorizationCache>(_ =>
        {
            var cache = new RequestAuthorizationCache();
            PopulateAuthorizationCache(cache);
            return cache;
        });
    }

    private static void PopulateAuthorizationCache(RequestAuthorizationCache cache)
    {
        var assemblies = new[]
        {
            typeof(ImplementationAssemblyMarker).Assembly,
            typeof(ApplicationAssemblyMarker).Assembly
        };

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract) continue;

                var attribute = type.GetCustomAttribute<AllowForRolesAttribute>();
                if (attribute is not null)
                {
                    cache.SetRoles(type, attribute.Roles);
                }
            }
        }
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
