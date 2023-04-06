namespace EFCoreExam.Request
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string UserId { get; set; }
    }
}
