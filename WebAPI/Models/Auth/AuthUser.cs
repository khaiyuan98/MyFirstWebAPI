namespace Test.WebAPI.Models.Auth
{
    public class AuthUser
    {
        public string AccessToken { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleId { get; set; }
    }
}
