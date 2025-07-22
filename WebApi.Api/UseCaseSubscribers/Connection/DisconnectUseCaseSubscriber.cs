using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Hubs;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Connection;
using WebApi.Common.DTO.Games;

namespace WebApi.Api.UseCaseSubscribers.Connection;

public class DisconnectUseCaseSubscriber(
    IHubContext<GameHub> _hubContext
) : IUseCaseSubscriber<DisconnectUseCase, string, DisconnectResult>
{
    public async Task OnUseCaseExecuted(UseCaseSubscriberData<string, DisconnectResult> data)
    {
        string? gameCode = data.UseCaseResult.Data!.GameCode;

        if (!string.IsNullOrEmpty(gameCode) && _hubContext?.Groups is not null)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(data.UseCaseData, gameCode);
        }
    }
}
