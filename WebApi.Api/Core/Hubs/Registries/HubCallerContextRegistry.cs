using Microsoft.AspNetCore.SignalR;

namespace WebApi.Api.Core.Hubs.Registries;

public class HubCallerContextRegistry
{
    public HubCallerContext Context { get; private set; }
    public IHubCallerClients Clients { get; private set; }

    public void SetContext(HubCallerContext context)
    {
        Context = context;
    }

    public void SetClients(IHubCallerClients clients)
    {
        Clients = clients;
    }
}
