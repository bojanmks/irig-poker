namespace WebApi.Application.ApplicationUsers;

public interface IUserSessionRegistry
{
    string ConnectionId { get; }
    void SetConnectionId(string connectionId);
}
