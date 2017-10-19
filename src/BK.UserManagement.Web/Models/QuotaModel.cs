using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models
{
    public class QuotaModel
    {
        public string TABLESPACE_NAME { get; set; }
        public string BYTES { get; set; }
        public string MAX_BYTES { get; set; }
    }
}
