using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class UserModel
    {
        public string USER_ID { get; set; }
        public string USERNAME { get; set; }
        //public string PASSWORD { get; set; }
        public string ACCOUNT_STATUS { get; set; }
        public string LOCK_DATE { get; set; }
        public string EXPIRE_DATE { get; set; }
        public string DEFAULT_TABLESPACE { get; set; }
        public string TEMPORARY_TABLESPACE { get; set; }
        public string PROFILE { get; set; }

        // TODO
    }
}
