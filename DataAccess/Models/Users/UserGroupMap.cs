using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DataAccess.Models.Users
{
    public class UserGroupMap
    {
        // properties from User_UserGroup_Map table
        public int UserId { get; set; }
        public int UserGroupId { get; set; }
    }
}