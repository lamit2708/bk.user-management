using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models.RoleViewModels
{
    public class GrantRoleToUserViewModel
    {
        

        public IEnumerable<SelectListItem> Users { get; set; }

        [Display(Name = "To")]
        public string Username { get; set; }

        [Display(Name = "Grant")]
        public string GrantedRole { get; set; }

    }
}

