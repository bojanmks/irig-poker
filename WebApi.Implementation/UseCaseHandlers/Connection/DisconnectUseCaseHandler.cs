using WebApi.Application.Games;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Connection;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCaseHandlers.Connection;

public class DisconnectUseCaseHandler(
    IDisconnectFromGameService _disconnectFromGameService    
) : UseCaseHandler<DisconnectUseCase, string, DisconnectResult>
{
    public override async Task<Result<DisconnectResult>> HandleAsync(DisconnectUseCase useCase, CancellationToken cancellationToken = default)
    {
        var result = await _disconnectFromGameService.DisconnectAsync(useCase.Data, cancellationToken);
        return Result<DisconnectResult>.Success(result);
    }
}
