using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DataAccess.Models.Users
{
    public class UpdateUser
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int UserRoleId { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public int LastUpdatedById { get; set; }
        public bool IsActive { get; set; }
        public List<int>? UserGroupIds { get; set; }
    }
}
