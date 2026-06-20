using System.Diagnostics.CodeAnalysis;
using WebApi.Application.Core.ApplicationUsers;

namespace WebApi.Implementation.Core.ApplicationUsers.Models;

public class HubConnectionIdProvider : IHubConnectionIdProvider, IHubConnectionIdSetter
{
    private string? _connectionId;

    public string GetConnectionId()
    {
        if (string.IsNullOrWhiteSpace(_connectionId))
        {
            throw new InvalidOperationException("Connection Id not set.");
        }

        return _connectionId;
    }

    public bool TryGetConnectionId([MaybeNullWhen(false)] out string connectionId)
    {
        connectionId = _connectionId;
        return !string.IsNullOrWhiteSpace(_connectionId);
    }

    public void SetConnectionId(string connectionId)
    {
        _connectionId = connectionId;
    }
}
