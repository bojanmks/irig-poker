using System.Globalization;
using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Application.Core.ApplicationUsers;

public interface IApplicationUser
{
    public UserRole Role { get; set; }
    public CultureInfo Locale { get; set; }
    public string? ConnectionId { get; set; }
    public string? GameCode { get; set; }
    public List<string> AllowedUseCases { get; set; }
}
