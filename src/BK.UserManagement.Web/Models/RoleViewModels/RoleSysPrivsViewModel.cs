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
        [Display(Name = "USER")]
        public string User { get; set; }

        [Required]
        [Display(Name = "CREATE PROFILE")]
        public bool CreateProfile { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool CreateProfileAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "ALTER PROFILE")]
        public bool AlterProfile { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool AlterProfileAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "DROP PROFILE")]
        public bool DropProfile { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool DropProfileAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "CREATE ROLE")]
        public bool CreateRole { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool CreateRoleAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "ALTER ANY ROLE")]
        public bool AlterAnyRole { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool AlterAnyRoleAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "DROP ANY ROLE")]
        public bool DropAnyRole { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool DropAnyRoleAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "GRANT ANY ROLE")]
        public bool GrantAnyRole { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool GrantAnyRoleAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "CREATE SESSION")]
        public bool CreateSession { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool CreateSessionAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "CREATE ANY TABLE")]
        public bool CreateAnyTable { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool CreateAnyTableAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "ALTER ANY TABLE")]
        public bool AlterAnyTable { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool AlterAnyTableAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "DROP ANY TABLE")]
        public bool DropAnyTable { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool DropAnyTableAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "SELECT ANY TABLE")]
        public bool SelectAnyTable { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool SelectAnyTableAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "DELETE ANY TABLE")]
        public bool DeleteAnyTable { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool DeleteAnyTableAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "INSERT ANY TABLE")]
        public bool InsertAnyTable { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool InsertAnyTableAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "UPDATE ANY TABLE")]
        public bool UpdateAnyTable { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool UpdateAnyTableAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "CREATE TABLE")]
        public bool CreateTable { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool CreateTableAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "CREATE USER")]
        public bool CreateUser { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool CreateUserAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "ALTER USER")]
        public bool AlterUser { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool AlterUserAdminOption { get; set; } = false;

        [Required]
        [Display(Name = "DROP USER")]
        public bool DropUser { get; set; } = false;
        [Required]
        [Display(Name = "ADMIN OPTION")]
        public bool DropUserAdminOption { get; set; } = false;

    }
}
