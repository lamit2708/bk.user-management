using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class ListRoleObjectPrivs
    {
        public string ROLE { get; set; }
        public string OWNER { get; set; }
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public string PRIVILEGE { get; set; }
        public string GRANTABLE { get; set; }
    }
}
