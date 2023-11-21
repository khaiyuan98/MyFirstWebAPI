namespace Test.WebAPI.Models.Auth
{
    public class RefreshToken
    {
        public string Token { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime Expires { get; set; }
    }
}
