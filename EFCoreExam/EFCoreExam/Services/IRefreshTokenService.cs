namespace EFCoreExam.Services
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken(string userId);
        bool ValidateRefreshToken(string userId, string token);
    }
}
