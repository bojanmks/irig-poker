using WebApi.Application.Core.UseCases;
using WebApi.Common.Core.Auth.Enums;
using WebApi.Common.Features.Games.Joining.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Application.Features.Games.UseCases;

[AllowForRoles(UserRole.NotPlaying)]
public class JoinGameUseCase : UseCase<JoinGameDto, PublicGameStateDto>
{
    public JoinGameUseCase(JoinGameDto data) : base(data)
    {
    }

    public override string Id => "JoinGameUseCase";
}
