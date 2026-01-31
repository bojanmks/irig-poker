using FluentValidation;
using System.Reflection;
using WebApi.Api.Core.Reflection.Extensions;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Core.Validation;
using WebApi.Common.Core.Auth.Enums;
using WebApi.Implementation.Core.Reflexion.Extensions;
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
        AddImplementationsByBaseType<IValidator>(services, [typeof(UserRoleUseCaseMapStore).Assembly]);
        services.AddTransient<IValidatorResolver, ServiceProviderValidatorResolver>();
    }

    private static void AddUseCaseHandlers(IServiceCollection services)
    {
        AddImplementationsByBaseType<IUseCaseHandlerBase>(services, [typeof(UserRoleUseCaseMapStore).Assembly]);
        services.AddTransient<IUseCaseHandlerResolver, ServiceProviderUseCaseHandlerResolver>();
    }

    private static void AddUseCaseSubscribers(IServiceCollection services)
    {
        var interfaceType = typeof(IUseCaseSubscriber<,,>);
        var assembliesToLookThrough = new Assembly[] { typeof(UserRoleUseCaseMapStore).Assembly, typeof(Program).Assembly };

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

        var useCaseTypes = typeof(UseCase<,>).Assembly.GetImplementationsOfGenericType(typeof(IUseCase<,>));

        foreach (var useCaseType in useCaseTypes)
        {
            var allowForRolesAttribute = useCaseType.GetAttributeOfType<AllowForRolesAttribute>();

            var allowedRoles = allowForRolesAttribute is null ? allRoles : allowForRolesAttribute.Roles;

            var constructorParameters = useCaseType.GetConstructors().OrderBy(x => x.GetParameters().Count()).FirstOrDefault()?.GetParameters();
            var constructorParametersDefaultValues = constructorParameters?.Select(x => x.GetType().GetDefault()).ToArray();

            var useCaseInstance = (IUseCaseBase?)Activator.CreateInstance(useCaseType, constructorParametersDefaultValues);

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

    private static void AddImplementationsByBaseType<T>(IServiceCollection services, Assembly[] assemblies)
    {
        var type = typeof(T);
        var types = assemblies.SelectMany(s => s.GetTypes())
                              .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract && !p.Name.Contains("Generic"));

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
