using System.Globalization;
using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Application.Core.ApplicationUsers;

public interface IApplicationUser
{
    public UserRole Role { get; }
    public CultureInfo Locale { get; }
    public string? ConnectionId { get; }
    public string? GameCode { get; }
}
