using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DataAccess.Models.Users
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public string? Description { get; set; }
    }

    public class FullUserRole
    {
        public int UserRoleId { get; set; }
        public string? Description { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }

    }

}
