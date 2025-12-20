namespace WebApi.Application.Core.ApplicationUsers;

public interface IApplicationUserResolver
{
    Task<IApplicationUser> ResolveAsync(CancellationToken cancellationToken = default);
    void SetConnectionId(string connectionId);
}
