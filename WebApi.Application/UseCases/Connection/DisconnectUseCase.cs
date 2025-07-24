using WebApi.Common.DTO.Games;

namespace WebApi.Application.UseCases.Connection;

public class DisconnectUseCase : UseCase<string, DisconnectResultDto>
{
    public DisconnectUseCase(string data) : base(data)
    {
    }

    public override string Id => "DisconnectFromGameUseCase";
}
