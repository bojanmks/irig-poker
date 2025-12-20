using WebApi.Application.Core.UseCases;
using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Application.Features.Games.UseCases;

[AllowForRoles(UserRole.NotPlaying)]
public class CreateGameUseCase : UseCase<Empty, string>
{
    public CreateGameUseCase(Empty data) : base(data)
    {
    }

    public override string Id => "CreateGameUseCase";
}