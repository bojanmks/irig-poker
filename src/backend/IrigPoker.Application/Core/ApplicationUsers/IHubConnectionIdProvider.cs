using System.Diagnostics.CodeAnalysis;

namespace IrigPoker.Application.Core.ApplicationUsers;

public interface IHubConnectionIdProvider
{
    string GetConnectionId();
    bool TryGetConnectionId([MaybeNullWhen(false)] out string connectionId);
}
