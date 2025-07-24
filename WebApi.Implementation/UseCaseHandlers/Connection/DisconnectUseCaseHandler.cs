using WebApi.Application.Games;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Connection;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCaseHandlers.Connection;

public class DisconnectUseCaseHandler(
    IDisconnectFromGameService _disconnectFromGameService    
) : UseCaseHandler<DisconnectUseCase, string, DisconnectResultDto>
{
    public override async Task<Result<DisconnectResultDto>> HandleAsync(DisconnectUseCase useCase, CancellationToken cancellationToken = default)
    {
        var result = await _disconnectFromGameService.DisconnectAsync(useCase.Data, cancellationToken);
        return Result<DisconnectResultDto>.Success(result);
    }
}
