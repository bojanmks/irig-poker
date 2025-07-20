using WebApi.Common.DTO.Games;

namespace WebApi.Application.UseCases.Games;

public class DisconnectFromGameUseCase : UseCase<string, DisconnectResult>
{
    public DisconnectFromGameUseCase(string data) : base(data)
    {
    }

    public override string Id => "DisconnectFromGameUseCase";
}
