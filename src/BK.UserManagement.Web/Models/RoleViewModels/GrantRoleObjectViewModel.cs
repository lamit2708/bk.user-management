using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models.RoleViewModels
{
    public class GrantRoleObjectViewModel
    {
        public IEnumerable<SelectListItem> RoleOb { get; set; }

        [Display(Name = "TABLE_NAME")]
        public string TABLENAME { get; set; }

        [Display(Name = "COLUMN_NAME ")]
        public string COLUMNNAME { get; set; }
    }
}
