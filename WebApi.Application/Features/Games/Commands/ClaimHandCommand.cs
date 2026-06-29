using MediatR;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Application.Features.Games.Commands;

public record ClaimHandCommand(ClaimHandRequest Data) : IRequest<Result<ClaimResult>>;
