using WebApi.Api.Core.Endpoints.Models;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Implementation.Core.UseCases.Execution;

namespace WebApi.Api.Features.Games.Endpoints;

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