using WebApi.Common.DTO.Games;
using WebApi.Common.Enums.Auth;

namespace WebApi.Application.UseCases.Games;

[AllowForRoles(UserRole.NotPlaying)]
public class JoinGameUseCase : UseCase<JoinGameDto, PublicGameStateDto>
{
    public JoinGameUseCase(JoinGameDto data) : base(data)
    {
    }

    public override string Id => "JoinGameUseCase";
}
