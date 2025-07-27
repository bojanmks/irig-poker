using System.Globalization;
using WebApi.Application.ApplicationUsers;
using WebApi.Common.Enums.Auth;

namespace WebApi.Implementation.ApplicationUsers
{
    public class ApplicationUser : IApplicationUser
    {
        public virtual UserRole Role { get; set; }
        public CultureInfo Locale { get; set; }
        public List<string> AllowedUseCases { get; set; } = new List<string>();
        public string? ConnectionId { get; set; }
        public string? GameCode { get; set; }
    }
}
