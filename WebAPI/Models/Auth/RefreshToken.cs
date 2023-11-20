namespace Test.WebAPI.Models.Auth
{
    public class RefreshToken
    {
        public string Token { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
