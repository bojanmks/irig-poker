using System.Reflection;

namespace WebApi.Implementation.Core.Reflection.Extensions;

public static class InheritanceExtensions
{
    public static IEnumerable<Type> GetInheritorsOfGenericClass(this Assembly assembly, Type genericClassType)
    {
        if (!genericClassType.IsGenericType || !genericClassType.IsClass)
            throw new ArgumentException("Provided type must be a generic class type.", nameof(genericClassType));

        return assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t =>
            {
                var baseType = t.BaseType;

                if (baseType is null)
                {
                    return false;
                }

                return baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericClassType;
            });
    }
}