using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Implementation.Core.Cqrs.Pipeline;

public interface IRequestAuthorizationCache
{
    UserRole[]? GetRoles(Type requestType);
}
