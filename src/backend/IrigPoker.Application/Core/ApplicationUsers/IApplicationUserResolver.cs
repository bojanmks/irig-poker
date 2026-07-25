namespace IrigPoker.Application.Core.ApplicationUsers;

public interface IApplicationUserResolver
{
    Task<IApplicationUser> ResolveAsync(CancellationToken cancellationToken = default);
}
