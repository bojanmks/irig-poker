using MediatR;
using IrigPoker.Application.Core.Cqrs;
using IrigPoker.Common.Core.Auth.Enums;
using IrigPoker.Common.Core.Result.Models;

namespace IrigPoker.Application.Features.Games.Commands;

[AllowForRoles(UserRole.NotPlaying)]
public record CreateGameCommand() : IRequest<Result<string>>;