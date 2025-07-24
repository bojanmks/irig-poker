using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Hubs;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Connection;
using WebApi.Common.DTO.Games;

namespace WebApi.Api.UseCaseSubscribers.Connection;

public class DisconnectUseCaseSubscriber(
    IHubContext<GameHub> _hubContext,
    HubCallerContextRegistry _hubCallerContextRegistry
) : IUseCaseSubscriber<DisconnectUseCase, string, DisconnectResultDto>
{
    public async Task OnUseCaseExecuted(UseCaseSubscriberData<string, DisconnectResultDto> data)
    {
        string? gameCode = data.UseCaseResult.Data!.GameCode;
        var callerContextClients = _hubCallerContextRegistry.Clients;

        if (!string.IsNullOrEmpty(gameCode) && _hubContext?.Groups is not null)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(data.UseCaseData, gameCode);
            await callerContextClients.GroupExcept(gameCode, data.UseCaseData).SendAsync("PlayerLeft", data.UseCaseData);
        }
    }
}
