using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models.DashboardViewModels
{
    public class DashboardViewModel
    {
        public int NumOfUsers { get; set; }

        public int NumOfRoles { get; set; }

        public int NumOfProfiles { get; set; }

        public int NumOfSessionPrivs { get; set; }

        public int NumOfSessionRoles { get; set; }

    }
}
