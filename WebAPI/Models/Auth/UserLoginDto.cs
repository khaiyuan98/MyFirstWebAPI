using System.ComponentModel.DataAnnotations;

namespace Test.WebAPI.Models.Auth
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
