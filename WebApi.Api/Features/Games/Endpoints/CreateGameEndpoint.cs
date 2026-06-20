using MediatR;
using WebApi.Api.Core.Endpoints.Models;
using WebApi.Application.Features.Games.Commands;
using WebApi.Common.Core.Cqrs;

namespace WebApi.Api.Features.Games.Endpoints;

public class CreateGameEndpoint(
    IMediator _mediator
) : BaseEndpoint<Empty, string>
{
    protected override void ConfigureEndpoint()
    {
        Post("/games/create");
    }

    public override async Task HandleAsync(Empty req, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateGameCommand(), ct);
        await RespondFromResult(result, ct);
    }
}
