using System.ComponentModel.DataAnnotations;

namespace Test.WebAPI.Models.User
{
    public class NewUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
