using MediatR;
using IrigPoker.Common.Core.Result.Models;
using IrigPoker.Common.Features.Games.Disconnecting.Models;

namespace IrigPoker.Application.Features.Games.Commands;

public record DisconnectCommand(string ConnectionId) : IRequest<Result<DisconnectResult>>;
