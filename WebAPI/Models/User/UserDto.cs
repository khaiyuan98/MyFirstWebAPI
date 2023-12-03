namespace Test.WebAPI.Models.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
