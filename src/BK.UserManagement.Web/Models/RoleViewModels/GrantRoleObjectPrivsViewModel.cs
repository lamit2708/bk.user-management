using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models.RoleViewModels
{
    public class GrantRoleObjectPrivsViewModel
    {

        [Required]
        [Display(Name = "ROLE NAME")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "TABLE_NAME")]
        public string Table_Name { get; set; }

        [Required]
        [Display(Name = "SELECT")]
        public bool Select { get; set; } = false;
     //   [Required]
     //   [Display(Name = "ADMIN OPTION")]
     //   public bool CreateProfileAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "INSERT")]
        public bool Insert { get; set; } = false;
    //    [Required]
    //    [Display(Name = "ADMIN OPTION")]
    //    public bool AlterProfileAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "DELETE")]
        public bool Delete { get; set; } = false;
    //    [Required]
    //    [Display(Name = "ADMIN OPTION")]
    //    public bool DropProfileAdminOption { get; set; } = false;

    }
}
