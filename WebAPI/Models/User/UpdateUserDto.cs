using System.ComponentModel.DataAnnotations;

namespace Test.WebAPI.Models.User
{
    public class UpdateUserDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public int UserRoleId { get; set; }

        public List<int>? UserGroupIds { get; set; }

        public string? Password { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
