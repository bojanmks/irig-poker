using MediatR;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Disconnecting.Models;

namespace WebApi.Application.Features.Games.Commands;

public record DisconnectCommand(string ConnectionId) : IRequest<Result<DisconnectResult>>;
