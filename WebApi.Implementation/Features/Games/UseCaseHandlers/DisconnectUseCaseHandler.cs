using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.Services;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Disconnecting.Models;

namespace WebApi.Implementation.Features.Games.UseCaseHandlers;

public class DisconnectUseCaseHandler(
    IDisconnectFromGameService _disconnectFromGameService
) : UseCaseHandler<DisconnectUseCase, string, DisconnectResultDto>
{
    public override async Task<Result<DisconnectResultDto>> HandleAsync(DisconnectUseCase useCase, CancellationToken cancellationToken = default)
    {
        var result = await _disconnectFromGameService.DisconnectAsync(useCase.Data, cancellationToken);
        return result;
    }
}
