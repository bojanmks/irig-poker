using MediatR;
using IrigPoker.Application.Core.Cqrs;
using IrigPoker.Common.Core.Auth.Enums;
using IrigPoker.Common.Core.Result.Models;
using IrigPoker.Common.Features.Games.Models;

namespace IrigPoker.Application.Features.Games.Commands;

[AllowForRoles(UserRole.RoomOwner, UserRole.Player)]
public record CallBluffCommand() : IRequest<Result<CallBluffResult>>;
