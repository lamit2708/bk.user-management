using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using BK.UserManagement.Web.Models;
using Dapper;
using BK.UserManagement.Web.Models.RoleViewModels;

namespace BK.UserManagement.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IConfiguration config;
        public RoleController(IConfiguration iconfig)
        {
            config = iconfig;
        }

        
        public IActionResult ListUserRole()
        {

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var userRole = ole.Query<UserRoleModel>("SELECT * FROM dba_role_privs");
                return View(userRole);
            }
        }
       
        public IActionResult Index()
        {

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var listRole = ole.Query<RoleSysPrivsModel>("SELECT * FROM ROLE_SYS_PRIVS");
                return View(listRole);
            }
        }

        public IActionResult EditRoleSysPrivs(string id)
        {
            var roleSysPrivsViewModel = new RoleSysPrivsViewModel();
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var roleCreateProfile = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='CREATE PROFILE'").FirstOrDefault();
                if (roleCreateProfile != null)
                {
                    roleSysPrivsViewModel.CreateProfile = true;
                }

                var roleCreateSession = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='CREATE SESSION'").FirstOrDefault();
                if (roleCreateSession != null)
               {
                    roleSysPrivsViewModel.CreateSession = true;
                }
                     
                return View(roleSysPrivsViewModel);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
                    {
                        conn.Open();
                        OracleCommand cmd = conn.CreateCommand();
                        //var user = ole.Query<UserModel>($"ALTER USER\"{model.Username.ToUpper()}\" IDENTIFIED BY \"{model.Password}\"");
                       
                       
                        if (!String.IsNullOrWhiteSpace(model.Password))
                        {
                            cmd.CommandText = $"CREATE ROLE {model.RoleName} IDENTIFIED BY {model.Password} ";
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = $"CREATE ROLE {model.RoleName}";

                            cmd.ExecuteNonQuery();
                        }

                        return RedirectToAction("Index", "Role");
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

            return RedirectToAction(nameof(UserController.Index), "User");
        }
    }
}