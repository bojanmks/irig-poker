using WebApi.Application.Games;
using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Common.DTO.Games;
using WebApi.Common.DTO.Players;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCaseHandlers.Games;

public class JoinGameUseCaseHandler(
    IGetGameService _getGameService
) : UseCaseHandler<JoinGameUseCase, JoinGameDto, Empty>
{
    public override async Task<Result<Empty>> HandleAsync(JoinGameUseCase useCase, CancellationToken cancellationToken = default)
    {
        string gameCode = useCase.Data.GameCode;

        var game = await _getGameService.GetAsync(gameCode);

        bool result = game!.Players.TryAdd(useCase.Data.ConnectionId, new PlayerDto
        {
            Username = useCase.Data.Username,
            IsAdmin = !game.Players.Any()
        });

        if (!result)
        {
            return Result<Empty>.Error();
        }

        return Result<Empty>.Success();
    }
}
