using System.Diagnostics.CodeAnalysis;

namespace WebApi.Application.Core.ApplicationUsers;

public interface IHubConnectionIdProvider
{
    string GetConnectionId();
    bool TryGetConnectionId([MaybeNullWhen(false)] out string connectionId);
}
