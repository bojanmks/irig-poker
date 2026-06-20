using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Application.Core.Cqrs;

public class AllowForRolesAttribute : Attribute
{
    private readonly UserRole[] _roles;

    public AllowForRolesAttribute(params UserRole[] roles)
    {
        _roles = roles;
    }

    public UserRole[] Roles => _roles;
}
