using System.Globalization;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Implementation.Core.ApplicationUsers.Models;

public class ApplicationUser : IApplicationUser
{
    public required UserRole Role { get; init; }
    public required CultureInfo Locale { get; init; }
    public required string? ConnectionId { get; init; }
    public required string? GameCode { get; init; }
}
