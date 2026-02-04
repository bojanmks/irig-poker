using Microsoft.AspNetCore.SignalR;

namespace WebApi.Api.Core.Hubs.Registries;

public interface IHubCallerContextAccessor
{
    HubCallerContext Context { get; }
    IHubCallerClients Clients { get; }
}

internal interface IHubCallerContextSetter
{
    void SetContext(HubCallerContext? context);
    void SetClients(IHubCallerClients? clients);
    void Clear();
}

internal sealed class HubCallerContextRegistry : IHubCallerContextAccessor, IHubCallerContextSetter
{
    private static readonly AsyncLocal<HubCallerContext?> _context = new();
    private static readonly AsyncLocal<IHubCallerClients?> _clients = new();

    public HubCallerContext Context => _context.Value!;
    public IHubCallerClients Clients => _clients.Value!;

    public void SetContext(HubCallerContext? context)
    {
        _context.Value = context;
    }

    public void SetClients(IHubCallerClients? clients)
    {
        _clients.Value = clients;
    }

    public void Clear()
    {
        _context.Value = null;
        _clients.Value = null;
    }
}