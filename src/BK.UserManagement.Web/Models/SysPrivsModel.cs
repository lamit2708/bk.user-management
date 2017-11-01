using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class SysPrivsModel
    {
        public string GRANTEE { get; set; }
        public string PRIVILEGE { get; set; }
        public string ADMIN_OPTION { get; set; }
    }
}
