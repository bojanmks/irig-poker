using System.Collections.Concurrent;
using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Implementation.Core.Cqrs.Pipeline;

public class RequestAuthorizationCache : IRequestAuthorizationCache
{
    private readonly ConcurrentDictionary<Type, UserRole[]> _cache = new();

    public UserRole[]? GetRoles(Type requestType)
    {
        return _cache.TryGetValue(requestType, out var roles) ? roles : null;
    }

    public void SetRoles(Type requestType, UserRole[] roles)
    {
        _cache[requestType] = roles;
    }
}
