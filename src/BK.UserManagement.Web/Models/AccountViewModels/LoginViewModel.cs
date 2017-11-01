﻿
using System.ComponentModel.DataAnnotations;

namespace BK.UserManagement.Web.Models.AccountViewModels
{

    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Server { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Sid { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }


}
