using WebApi.Application.Core.UseCases;
using WebApi.Common.Features.Games.Disconnecting.Models;

namespace WebApi.Application.Features.Games.UseCases;

public class DisconnectUseCase : UseCase<string, DisconnectResultDto>
{
    public DisconnectUseCase(string data) : base(data)
    {
    }

    public override string Id => "DisconnectFromGameUseCase";
}
