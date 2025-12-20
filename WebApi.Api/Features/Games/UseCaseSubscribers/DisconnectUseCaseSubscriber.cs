using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.Hubs.Models;
using WebApi.Api.Core.Hubs.Registries;
using WebApi.Api.Features.Games.Hubs;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Common.Features.Games.Disconnecting.Models;

namespace WebApi.Api.Features.Games.UseCaseSubscribers;

public class DisconnectUseCaseSubscriber(
    IHubContext<GameHub> _hubContext,
    HubCallerContextRegistry _hubCallerContextRegistry
) : IUseCaseSubscriber<DisconnectUseCase, string, DisconnectResultDto>
{
    public async Task ExecuteAsync(UseCaseSubscriberData<string, DisconnectResultDto> data, CancellationToken cancellationToken = default)
    {
        string? gameCode = data.UseCaseResult.Data!.GameCode;
        var callerContextClients = _hubCallerContextRegistry.Clients;

        if (!string.IsNullOrEmpty(gameCode) && _hubContext?.Groups is not null)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(data.UseCaseData, gameCode, cancellationToken);
            await callerContextClients.GroupExcept(gameCode, data.UseCaseData).SendAsync("PlayerLeft", HubNotification.From(data.UseCaseData), cancellationToken);
        }
    }
}
