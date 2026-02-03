using System.Globalization;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Common.Core.Auth.Enums;

namespace WebApi.Implementation.Core.ApplicationUsers.Models;

public class ApplicationUser : IApplicationUser
{
    public virtual UserRole Role { get; set; }
    public CultureInfo Locale { get; set; }
    public List<string> AllowedUseCases { get; set; } = new List<string>();
    public string? ConnectionId { get; set; }
    public string? GameCode { get; set; }
}
