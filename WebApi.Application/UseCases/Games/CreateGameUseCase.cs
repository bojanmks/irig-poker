using WebApi.Common.Enums.Auth;

namespace WebApi.Application.UseCases.Games;

[AllowForRoles(UserRole.NotPlaying)]
public class CreateGameUseCase : UseCase<Empty, string>
{
    public CreateGameUseCase(Empty data) : base(data)
    {
    }

    public override string Id => "CreateGameUseCase";
}