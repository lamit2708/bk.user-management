using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models.RoleViewModels
{
    public class RoleSysPrivsViewModel
    {
        [Required]
        [Display(Name = "CREATE PROFILE")]
        public bool CreateProfile { get; set; } = false;

        [Required]
        [Display(Name = "CREATE SESSION")]
        public bool CreateSession { get; set; } = false;




    }
}
