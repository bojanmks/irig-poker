using WebApi.Common.Enums.Auth;

namespace WebApi.Application.UseCases.Games;

[AllowForRoles(UserRole.RoomOwner)]
public class StartGameUseCase : UseCase<Empty, Empty>
{
    public StartGameUseCase(Empty data) : base(data)
    {
    }

    public override string Id => "StartGameUseCase";
}
