namespace WebApi.Application.ApplicationUsers;

public interface IApplicationUserResolver
{
    Task<IApplicationUser> ResolveAsync(CancellationToken cancellationToken = default);
}
