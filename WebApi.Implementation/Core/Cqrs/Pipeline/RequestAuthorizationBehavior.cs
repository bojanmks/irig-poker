using MediatR;
using System.Net;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Core.Cqrs.Pipeline;

public class RequestAuthorizationBehavior<TRequest, TInner> : IPipelineBehavior<TRequest, Result<TInner>>
    where TRequest : IRequest<Result<TInner>>
{
    private readonly IApplicationUserResolver _applicationUserResolver;
    private readonly IRequestAuthorizationCache _authorizationCache;

    public RequestAuthorizationBehavior(IApplicationUserResolver applicationUserResolver, IRequestAuthorizationCache authorizationCache)
    {
        _applicationUserResolver = applicationUserResolver;
        _authorizationCache = authorizationCache;
    }

    public async Task<Result<TInner>> Handle(TRequest request, RequestHandlerDelegate<Result<TInner>> next, CancellationToken cancellationToken)
    {
        var roles = _authorizationCache.GetRoles(typeof(TRequest));

        if (roles is not null)
        {
            var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

            if (!roles.Contains(applicationUser.Role))
            {
                return Result<TInner>.Error()
                    .WithHttpStatusCode((int)HttpStatusCode.Forbidden);
            }
        }

        return await next();
    }
}
