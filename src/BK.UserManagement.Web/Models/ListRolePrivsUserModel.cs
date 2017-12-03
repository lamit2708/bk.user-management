using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class ListRolePrivsUserModel
    {
        public string ROLE { get; set; }
        public string PRIVILEGE { get; set; }
        public string GRANTEE { get; set; }

    }
}
