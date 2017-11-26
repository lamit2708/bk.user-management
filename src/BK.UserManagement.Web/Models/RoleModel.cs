using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class RoleModel
    {
        public string ROLE { get; set; }
        public string PASSWORD_REQUIRED { get; set; }
        public string AUTHENTICATION_TYPE { get; set; }
    }
}
