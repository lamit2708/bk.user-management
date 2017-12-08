using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class UserInfoModel
    {
        public string USER_ID { get; set; }
        public string USERNAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
    }
}
