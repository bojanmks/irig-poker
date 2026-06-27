using MediatR;
using WebApi.Application.Core.Cqrs;
using WebApi.Common.Core.Auth.Enums;
using WebApi.Common.Core.Result.Models;
using WebApi.Common.Features.Games.Models;

namespace WebApi.Application.Features.Games.Commands;

[AllowForRoles(UserRole.RoomOwner)]
public record StartGameCommand() : IRequest<Result<StartGameResult>>;