using MediatR;
using System.Net;
using System.Reflection;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Cqrs;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Core.Cqrs.Pipeline;

public class RequestAuthorizationBehavior<TRequest, TInner> : IPipelineBehavior<TRequest, Result<TInner>>
    where TRequest : IRequest<Result<TInner>>
{
    private readonly IApplicationUserResolver _applicationUserResolver;

    public RequestAuthorizationBehavior(IApplicationUserResolver applicationUserResolver)
    {
        _applicationUserResolver = applicationUserResolver;
    }

    public async Task<Result<TInner>> Handle(TRequest request, RequestHandlerDelegate<Result<TInner>> next, CancellationToken cancellationToken)
    {
        var allowForRoles = typeof(TRequest).GetCustomAttribute<AllowForRolesAttribute>();

        if (allowForRoles is not null)
        {
            var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

            if (!allowForRoles.Roles.Contains(applicationUser.Role))
            {
                return Result<TInner>.Error()
                    .WithHttpStatusCode((int)HttpStatusCode.Forbidden);
            }
        }

        return await next();
    }
}
