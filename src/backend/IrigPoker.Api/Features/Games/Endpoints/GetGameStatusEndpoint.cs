using MediatR;
using IrigPoker.Api.Core.Endpoints.Models;
using IrigPoker.Application.Features.Games.Queries;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Api.Features.Games.Endpoints;

public record GetGameStatusRequest(string GameCode);

public class GetGameStatusEndpoint(
    IMediator _mediator
) : BaseEndpoint<GetGameStatusRequest, GameStatusResult>
{
    protected override void ConfigureEndpoint()
    {
        Get("/games/{gameCode}/status");
    }

    public override async Task HandleAsync(GetGameStatusRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetGameStatusQuery(req.GameCode), ct);
        await RespondFromResult(result, ct);
    }
}
