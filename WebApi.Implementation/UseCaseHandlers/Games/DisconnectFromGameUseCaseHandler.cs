using WebApi.Application.Games;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCaseHandlers.Games;

public class DisconnectFromGameUseCaseHandler(
    IDisconnectFromGameService _disconnectFromGameService    
) : UseCaseHandler<DisconnectFromGameUseCase, string, DisconnectResult>
{
    public override async Task<Result<DisconnectResult>> HandleAsync(DisconnectFromGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        var result = await _disconnectFromGameService.DisconnectAsync(useCase.Data, cancellationToken);
        return Result<DisconnectResult>.Success(result);
    }
}
