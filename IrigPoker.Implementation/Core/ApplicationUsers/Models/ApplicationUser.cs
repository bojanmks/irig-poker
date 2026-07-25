using System.Globalization;
using IrigPoker.Application.Core.ApplicationUsers;
using IrigPoker.Common.Core.Auth.Enums;

namespace IrigPoker.Implementation.Core.ApplicationUsers.Models;

public class ApplicationUser : IApplicationUser
{
    public required UserRole Role { get; init; }
    public required CultureInfo Locale { get; init; }
    public required string? GameCode { get; init; }
    public required string? PlayerId { get; init; }
}
