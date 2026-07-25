using MediatR;
using IrigPoker.Api.Core.Endpoints.Models;
using IrigPoker.Application.Features.Games.Commands;
using IrigPoker.Common.Core.Cqrs;

namespace IrigPoker.Api.Features.Games.Endpoints;

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
