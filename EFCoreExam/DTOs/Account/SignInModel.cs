using System.ComponentModel.DataAnnotations;

namespace EFCoreExam.DTOs.Account
{
    public class SignInModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
