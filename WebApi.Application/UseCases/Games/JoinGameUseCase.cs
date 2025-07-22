using WebApi.Common.DTO.Games;

namespace WebApi.Application.UseCases.Games;

public class JoinGameUseCase : UseCase<JoinGameDto, PublicGameState>
{
    public JoinGameUseCase(JoinGameDto data) : base(data)
    {
    }

    public override string Id => "JoinGameUseCase";
}
