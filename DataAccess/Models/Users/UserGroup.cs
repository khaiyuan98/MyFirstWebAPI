using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DataAccess.Models.Users
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public string? Description { get; set; }
    }

    public class FullUserGroup
    {
        public int UserGroupId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
