using MediatR;
using IrigPoker.Common.Core.Result.Models;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Application.Features.Games.Queries;

public record GetGameStatusQuery(string GameCode) : IRequest<Result<GameStatusResult>>;
