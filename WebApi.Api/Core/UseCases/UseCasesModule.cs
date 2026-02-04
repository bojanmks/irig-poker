using FluentValidation;
using System.Reflection;
using WebApi.Api.Core.Modules;
using WebApi.Api.Core.Reflection.Extensions;
using WebApi.Application;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Core.Validation;
using WebApi.Common.Core.Auth.Enums;
using WebApi.Implementation;
using WebApi.Implementation.Core.Reflection.Extensions;
using WebApi.Implementation.Core.UseCases.Execution;
using WebApi.Implementation.Core.UseCases.Resolvers;
using WebApi.Implementation.Core.UseCases.Stores;
using WebApi.Implementation.Core.Validation.Resolvers;

namespace WebApi.Api.Core.UseCases;

public class UseCasesModule : BaseModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<UseCaseMediator>();

        AddUseCaseHandlers(services);
        AddUseCaseValidators(services);
        AddUseCaseSubscribers(services);
        AddUserRoleUseCaseMapStore(services);
    }

    private static void AddUseCaseValidators(IServiceCollection services)
    {
        var validatorTypes = typeof(ImplementationAssemblyMarker).Assembly
            .GetTypes()
            .Where(p => typeof(IValidator).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract && !p.Name.Contains("Generic"))
            .ToArray();

        RegisterAsBaseTypes(services, validatorTypes);
        services.AddTransient<IValidatorResolver, ServiceProviderValidatorResolver>();
    }

    private static void AddUseCaseHandlers(IServiceCollection services)
    {
        var handlerTypes = typeof(ImplementationAssemblyMarker).Assembly
            .GetInheritorsOfGenericClass(typeof(UseCaseHandler<,,>))
            .Where(x => !x.Name.Contains("Generic"))
            .ToArray();

        RegisterAsBaseTypes(services, handlerTypes);
        services.AddTransient<IUseCaseHandlerResolver, ServiceProviderUseCaseHandlerResolver>();
    }

    private static void AddUseCaseSubscribers(IServiceCollection services)
    {
        var interfaceType = typeof(IUseCaseSubscriber<,,>);
        var assembliesToLookThrough = new Assembly[] { typeof(ImplementationAssemblyMarker).Assembly, typeof(ApiAssemblyMarker).Assembly };

        var useCaseSubscribersTypesData = interfaceType.GetGenericInterfaceImplementationTypes(assembliesToLookThrough);

        var groupedUseCaseSubscribersTypesData = useCaseSubscribersTypesData.GroupBy(x => x.ImplementedInterface).Select(x => x.AsEnumerable());

        foreach (var typesGroup in groupedUseCaseSubscribersTypesData)
        {
            foreach (var typeData in typesGroup)
            {
                services.AddTransient(typeData.ImplementedInterface, typeData.ImplementationType);
            }
        }

        services.AddTransient<IUseCaseSubscriberResolver, ServiceProviderUseCaseSubscriberResolver>();
    }

    private static void AddUserRoleUseCaseMapStore(IServiceCollection services)
    {
        var allRoles = Enum.GetValues<UserRole>();

        var useCasesMap = new Dictionary<UserRole, List<string>>();

        foreach (var role in allRoles)
        {
            useCasesMap.Add(role, new List<string>());
        }

        var useCaseTypes = typeof(ApplicationAssemblyMarker).Assembly.GetImplementationsOfGenericInterface(typeof(IUseCase<,>));

        foreach (var useCaseType in useCaseTypes)
        {
            var allowForRolesAttribute = useCaseType.GetAttributeOfType<AllowForRolesAttribute>();

            var allowedRoles = allowForRolesAttribute is null ? allRoles : allowForRolesAttribute.Roles;

            var constructorParameters = useCaseType.GetConstructors().OrderBy(x => x.GetParameters().Count()).FirstOrDefault()?.GetParameters();
            var constructorParametersDefaultValues = constructorParameters?.Select(x => x.GetType().GetDefault()).ToArray();

            var useCaseInstance = (IIdentifyable?)Activator.CreateInstance(useCaseType, constructorParametersDefaultValues);

            if (useCaseInstance is null)
            {
                continue;
            }

            foreach (var allowedRole in allowedRoles)
            {
                useCasesMap[allowedRole].Add(useCaseInstance.Id);
            }
        }

        var store = new UserRoleUseCaseMapStore(useCasesMap);

        services.AddSingleton(store);
    }

    private static void RegisterAsBaseTypes(IServiceCollection services, IEnumerable<Type> types)
    {
        foreach (var t in types)
        {
            var baseType = t.BaseType;

            if (baseType != null)
            {
                while (baseType.BaseType != null && baseType.BaseType.FullName != "System.Object")
                {
                    baseType = baseType.BaseType;
                }

                services.AddTransient(baseType, t);
            }
        }
    }
}
