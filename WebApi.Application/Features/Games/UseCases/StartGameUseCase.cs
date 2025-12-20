using WebApi.Application.Core.UseCases;
using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Application.Features.Games.UseCases;

[AllowForRoles(UserRole.RoomOwner)]
public class StartGameUseCase : UseCase<Empty, Empty>
{
    public StartGameUseCase(Empty data) : base(data)
    {
    }

    public override string Id => "StartGameUseCase";
}
