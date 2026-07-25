using System.Globalization;
using IrigPoker.Common.Core.Auth.Enums;

namespace IrigPoker.Application.Core.ApplicationUsers;

public interface IApplicationUser
{
    public UserRole Role { get; }
    public CultureInfo Locale { get; }
    public string? GameCode { get; }
    public string? PlayerId { get; }
}
