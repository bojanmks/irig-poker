using WebApi.Application.ApplicationUsers;

namespace WebApi.Implementation.ApplicationUsers;

public class UserSessionRegistry : IUserSessionRegistry
{
    public string ConnectionId { get; private set; } = string.Empty;

    public void SetConnectionId(string connectionId)
    {
        ConnectionId = connectionId;
    }
}
