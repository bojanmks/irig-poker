using System.Reflection;

namespace WebApi.Implementation.Core.Reflection.Extensions;

public static class InterfaceImplementationExtensions
{
    public static IEnumerable<Type> GetImplementationsOfGenericInterface(this Assembly assembly, Type genericInterfaceType)
    {
        if (!genericInterfaceType.IsGenericType || !genericInterfaceType.IsInterface)
            throw new ArgumentException("Provided type must be a generic interface type.", nameof(genericInterfaceType));

        return assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType));
    }
}
