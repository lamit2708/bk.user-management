using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class ProfileResource
    {
        public string Profile { get; set; }
        public string Resource_Name { get; set; }
        public string Resource_Type { get; set; }
        public string Limit { get; set; }
    }
}
