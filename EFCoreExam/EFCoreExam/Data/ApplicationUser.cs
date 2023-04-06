using Microsoft.AspNetCore.Identity;

namespace EFCoreExam.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
