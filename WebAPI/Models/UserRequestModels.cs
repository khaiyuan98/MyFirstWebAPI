namespace Test.WebAPI.Models
{
    public class NewUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
