using System.ComponentModel.DataAnnotations;

namespace Test.WebAPI.Models.User
{
    public class NewUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
