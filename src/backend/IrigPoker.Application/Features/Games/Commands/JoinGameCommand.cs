using MediatR;
using IrigPoker.Application.Core.Cqrs;
using IrigPoker.Common.Core.Auth.Enums;
using IrigPoker.Common.Core.Result.Models;
using IrigPoker.Common.Features.Games.Joining.Models;

namespace IrigPoker.Application.Features.Games.Commands;

[AllowForRoles(UserRole.NotPlaying)]
public record JoinGameCommand(JoinGameRequest Data) : IRequest<Result<JoinGameResult>>;