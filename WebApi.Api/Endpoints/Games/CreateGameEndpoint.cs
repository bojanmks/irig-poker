using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Games;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Endpoints.Games;

public class CreateGameEndpoint(
    UseCaseMediator _mediator
) : BaseEndpoint<Empty, string>
{
    protected override void ConfigureEndpoint()
    {
        Post("/games/create");
    }

    public override async Task HandleAsync(Empty req, CancellationToken ct)
    {
        var result = await _mediator.Execute<CreateGameUseCase, Empty, string>(new CreateGameUseCase(req));
        await RespondFromResult(result, ct);
    }
}