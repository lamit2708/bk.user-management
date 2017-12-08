using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BK.UserManagement.Web.Models.UserViewModels
{
    public class EditUserViewModel
    {
        public UserModel User { get; set; }
        public IEnumerable<SelectListItem> Tablespaces { get; set; }
        public IEnumerable<SelectListItem> Profiles { get; set; }
        public IEnumerable<SelectListItem> AccoutStatusList { get; set; }
        public IEnumerable<QuotaModel> QuotaList { get; set; }

        [Display(Name = "Profile")]
        public string ProfileName { get; set; }

        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Default Tablespace")]
        public string DefaultTablespaceName { get; set; }

        [Display(Name = "Temp Tablespace")]
        public string TemporaryTablespaceName { get; set; }

        [Display(Name = "Account Status")]
        public string AccountStatus { get; set; }

        [Display(Name = "Grant Quota")]
        public int Quota { get; set; }

        [Display(Name = "Quota Tablespace")]
        public string QuotaTablespace { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }


    }
}
