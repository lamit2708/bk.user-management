using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class UserRoleModel
    {
        public string GRANTEE { get; set; }
        public string GRANTED_ROLE { get; set; }
        public string ADMIN_OPTION { get; set; }
        public string DEFAULT_ROLE { get; set; }

    }
}
