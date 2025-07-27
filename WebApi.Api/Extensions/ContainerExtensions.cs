using System.Data;
using System.Reflection;
using FluentValidation;
using WebApi.Api.Hubs;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.AppSettings;
using WebApi.Application.Games;
using WebApi.Application.Localization;
using WebApi.Application.Logging;
using WebApi.Application.UseCases;
using WebApi.Application.Validation;
using WebApi.Common.Enums.Auth;
using WebApi.Implementation.ApplicationUsers;
using WebApi.Implementation.Extensions;
using WebApi.Implementation.Games;
using WebApi.Implementation.Localization;
using WebApi.Implementation.Logging;
using WebApi.Implementation.Players;
using WebApi.Implementation.UseCases;
using WebApi.Implementation.Validators;

namespace WebApi.Api.Extensions;

public static class ContainerExtensions
{
    public static void SetupApplication(this WebApplicationBuilder builder)
    {
        var appSettings = new AppSettings();
        builder.Configuration.Bind(appSettings);

        builder.Services.AddMemoryCache();
        builder.Services.RegisterDependencies(appSettings);
    }

    private static void RegisterDependencies(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton(appSettings);

        services.AddScoped<HubCallerContextRegistry>();

        services.AddScoped<ILocaleResolver, LocaleResolver>();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddApplicationUser();
        services.AddUseCases();
        services.SetupLocalization();

        services.AddTransient<IUseCaseLogger, ConsoleUseCaseLogger>();
        services.AddTransient<IExceptionLogger, ConsoleExceptionLogger>();

        // Games
        services.AddSingleton<GameStore>();
        services.AddSingleton<PlayersGamesMap>();
        services.AddTransient<ICreateGameService, CreateGameService>();
        services.AddTransient<IGetGameService, GetGameService>();
        services.AddTransient<IDeleteGameService, DeleteGameService>();
        services.AddTransient<IGameExistsService, GameExistsService>();
        services.AddTransient<IAddPlayerToGameService, AddPlayerToGameService>();
        services.AddTransient<IDisconnectFromGameService, DisconnectFromGameService>();
        services.AddTransient<IStartGameService, StartGameService>();
    }

    private static void AddApplicationUser(this IServiceCollection services)
    {
        services.AddScoped<IUserSessionRegistry, UserSessionRegistry>();
        services.AddTransient<IApplicationUserResolver, ApplicationUserResolver>();
    }

    #region UseCases
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddTransient<UseCaseMediator>();

        services.AddUseCaseHandlers();
        services.AddUseCaseValidators();
        services.AddUseCaseSubscribers();
        services.AddUserRoleUseCaseMapStore();
    }

    private static void AddUseCaseValidators(this IServiceCollection services)
    {
        services.AddImplementationsByBaseType<IValidator>([typeof(UserRoleUseCaseMapStore).Assembly]);
        services.AddTransient<IValidatorResolver, ServiceProviderValidatorResolver>();
    }

    private static void AddUseCaseHandlers(this IServiceCollection services)
    {
        services.AddImplementationsByBaseType<IUseCaseHandlerBase>([typeof(UserRoleUseCaseMapStore).Assembly]);
        services.AddTransient<IUseCaseHandlerResolver, ServiceProviderUseCaseHandlerResolver>();
    }

    private static void AddUseCaseSubscribers(this IServiceCollection services)
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

    private static void AddUserRoleUseCaseMapStore(this IServiceCollection services)
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
    #endregion

    private static void AddImplementationsByBaseType<T>(this IServiceCollection services, Assembly[] assemblies)
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

    private static void SetupLocalization(this IServiceCollection services)
    {
        services.AddScoped<ITranslator, JsonTranslator>();
    }
}
