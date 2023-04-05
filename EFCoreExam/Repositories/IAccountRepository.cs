using EFCoreExam.DTOs.Account;
using Microsoft.AspNetCore.Identity;

namespace EFCoreExam.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpSync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
    }
}
