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

        // Show List Role
        public IActionResult Index()
        {

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var listRole = ole.Query<RoleModel>("SELECT * FROM DBA_ROLES");
                return View(listRole);
            }
        }
        // Edit Pass Role
        [HttpGet]
        public IActionResult EditRole()
        {

            return View();
        }


        [HttpPost]
        public IActionResult EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
                    {
                        conn.Open();
                        OracleCommand cmd = conn.CreateCommand();

                        if (!String.IsNullOrWhiteSpace(model.Password))
                        {
                            cmd.CommandText = $"ALTER ROLE {model._RoleName} IDENTIFIED BY {model.Password} ";
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = $"ALTER ROLE {model._RoleName} NOT IDENTIFIED";

                            cmd.ExecuteNonQuery();
                        }

                        return RedirectToAction("Index", "Role");
                    }
                }
                catch (Exception e)
                {

                }

            }

            return RedirectToAction(nameof(RoleController.Index), "Role");
        }

        // DELETE ROLE



        // List Role Role Privs

        public IActionResult ListRoleRolePrivs()
        {

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var listRoleRole = ole.Query<RoleRolePrivsModel>("SELECT * FROM ROLE_ROLE_PRIVS");
                return View(listRoleRole);
            }
        }

        //List User Role

        public IActionResult ListUserRole()
        {

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var userRole = ole.Query<UserRoleModel>("SELECT * FROM DBA_ROLE_PRIVS");
                return View(userRole);
            }
            
        }

        


        // List Role Sys Privs
        public IActionResult ListRoleSysPrivs()
        {

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var roleSys = ole.Query<RoleSysPrivsModel>("SELECT * FROM ROLE_SYS_PRIVS");
                return View(roleSys);
            }
        }


        [HttpGet]
        public IActionResult EditRoleSysPrivs(string id)
        {
            var roleSysPrivsViewModel = new RoleSysPrivsViewModel();
            roleSysPrivsViewModel.User = id;
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var roleCreateProfile = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='CREATE PROFILE'").FirstOrDefault();
                if (roleCreateProfile != null)
                {
                    if (roleCreateProfile.ADMIN_OPTION == "YES") roleSysPrivsViewModel.CreateProfileAdminOption = true;
                    roleSysPrivsViewModel.CreateProfile = true;
                }

                var roleAlterProfile = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='ALTER PROFILE'").FirstOrDefault();
                if (roleAlterProfile != null)
                {
                    if (roleAlterProfile.ADMIN_OPTION=="YES") roleSysPrivsViewModel.AlterProfileAdminOption =true ;
                    roleSysPrivsViewModel.AlterProfile = true;
                }

                var roleDropProfile = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='DROP PROFILE'").FirstOrDefault();
                if (roleDropProfile != null)
                {
                    if (roleDropProfile.ADMIN_OPTION=="YES") roleSysPrivsViewModel.DropProfileAdminOption =true ;
                    roleSysPrivsViewModel.DropProfile = true;
                }

                var roleCreateRole = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='CREATE ROLE'").FirstOrDefault();
                if (roleCreateRole != null)
                {
                    if (roleCreateRole.ADMIN_OPTION=="YES") roleSysPrivsViewModel.CreateRoleAdminOption = true ;
                    roleSysPrivsViewModel.CreateRole = true;
                }

                var roleAlterAnyRole = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='ALTER ANY ROLE'").FirstOrDefault();
                if (roleAlterAnyRole != null)
                {
                    if (roleAlterAnyRole.ADMIN_OPTION=="YES") roleSysPrivsViewModel.AlterAnyRoleAdminOption = true ;
                    roleSysPrivsViewModel.AlterAnyRole = true;
                }

                var roleDropAnyRole = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='DROP ANY ROLE'").FirstOrDefault();
                if (roleDropAnyRole != null)
                {
                    if (roleDropAnyRole.ADMIN_OPTION=="YES") roleSysPrivsViewModel.DropAnyRoleAdminOption = true ;
                    roleSysPrivsViewModel.DropAnyRole = true;
                }

                var roleGrantAnyRole = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='GRANT ANY ROLE'").FirstOrDefault();
                if (roleGrantAnyRole != null)
                {
                    if (roleGrantAnyRole.ADMIN_OPTION=="YES") roleSysPrivsViewModel.GrantAnyRoleAdminOption = true ;
                    roleSysPrivsViewModel.GrantAnyRole = true;
                }

                var roleCreateSession = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='CREATE SESSION'").FirstOrDefault();
                if (roleCreateSession != null)
                {
                    if (roleCreateSession.ADMIN_OPTION=="YES") roleSysPrivsViewModel.CreateSessionAdminOption = true ;
                    roleSysPrivsViewModel.CreateSession = true;
                }

                var roleCreateAnyTable = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='CREATE ANY TABLE'").FirstOrDefault();
                if (roleCreateAnyTable != null)
                {
                    if (roleCreateAnyTable.ADMIN_OPTION=="YES") roleSysPrivsViewModel.CreateAnyTableAdminOption = true ;
                    roleSysPrivsViewModel.CreateAnyTable = true;
                }

                var roleAlterAnyTable = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='ALTER ANY TABLE'").FirstOrDefault();
                if (roleAlterAnyTable != null)
                {
                    if (roleAlterAnyTable.ADMIN_OPTION=="YES") roleSysPrivsViewModel.AlterAnyTableAdminOption = true ;
                    roleSysPrivsViewModel.AlterAnyTable = true;
                }

                var roleDropAnyTable = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='DROP ANY TABLE'").FirstOrDefault();
                if (roleDropAnyTable != null)
                {
                    if (roleDropAnyTable.ADMIN_OPTION=="YES") roleSysPrivsViewModel.DropAnyTableAdminOption = true ;
                    roleSysPrivsViewModel.DropAnyTable = true;
                }

                var roleSelectAnyTable = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='SELECT ANY TABLE'").FirstOrDefault();
                if (roleSelectAnyTable != null)
                {
                    if (roleSelectAnyTable.ADMIN_OPTION=="YES") roleSysPrivsViewModel.SelectAnyTableAdminOption = true ;
                    roleSysPrivsViewModel.SelectAnyTable = true;
                }

                var roleDeleteAnyTable = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='DELETE ANY TABLE'").FirstOrDefault();
                if (roleDeleteAnyTable != null)
                {
                    if (roleDeleteAnyTable.ADMIN_OPTION=="YES") roleSysPrivsViewModel.DeleteAnyTableAdminOption = true ;
                    roleSysPrivsViewModel.DeleteAnyTable = true;
                }

                var roleInsertAnyTable = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='INSERT ANY TABLE'").FirstOrDefault();
                if (roleInsertAnyTable != null)
                {
                    if (roleInsertAnyTable.ADMIN_OPTION=="YES") roleSysPrivsViewModel.InsertAnyTableAdminOption = true ;
                    roleSysPrivsViewModel.InsertAnyTable = true;
                }

                var roleUpdateAnyTable = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='UPDATE ANY TABLE'").FirstOrDefault();
                if (roleUpdateAnyTable != null)
                {
                    if (roleUpdateAnyTable.ADMIN_OPTION=="YES") roleSysPrivsViewModel.UpdateAnyTableAdminOption = true ;
                    roleSysPrivsViewModel.UpdateAnyTable = true;
                }

                var roleCreateTable = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='CREATE TABLE'").FirstOrDefault();
                if (roleCreateTable != null)
                {
                    if (roleCreateTable.ADMIN_OPTION=="YES") roleSysPrivsViewModel.CreateTableAdminOption = true ;
                    roleSysPrivsViewModel.CreateTable = true;
                }

                var roleCreateUser = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='CREATE USER'").FirstOrDefault();
                if (roleCreateUser != null)
                {
                    if (roleCreateUser.ADMIN_OPTION=="YES") roleSysPrivsViewModel.CreateUserAdminOption = true ;
                    roleSysPrivsViewModel.CreateUser = true;
                }

                var roleAlterUser = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='ALTER USER'").FirstOrDefault();
                if (roleAlterUser != null)
                {
                    if (roleAlterUser.ADMIN_OPTION=="YES") roleSysPrivsViewModel.AlterUserAdminOption = true ;
                    roleSysPrivsViewModel.AlterUser = true;
                }

                var roleDropUser = ole.Query<RoleSysPrivsModel>($"select * from ROLE_SYS_PRIVS where role='{id}' and PRIVILEGE='DROP USER'").FirstOrDefault();
                if (roleDropUser != null)
                {
                    if (roleDropUser.ADMIN_OPTION=="YES") roleSysPrivsViewModel.DropUserAdminOption = true ;
                    roleSysPrivsViewModel.DropUser = true;
                }

                return View(roleSysPrivsViewModel);
            }
        }
        [HttpPost]
        public IActionResult EditRoleSysPrivs(RoleSysPrivsViewModel model)
        {
            if (ModelState.IsValid)
            {
                EditSysPrivs(model.User, "CREATE PROFILE", model.CreateProfile, model.CreateProfileAdminOption);
                EditSysPrivs(model.User, "ALTER PROFILE", model.AlterProfile, model.AlterProfileAdminOption);
                EditSysPrivs(model.User, "DROP PROFILE", model.DropProfile, model.DropProfileAdminOption);
                EditSysPrivs(model.User, "CREATE ROLE", model.CreateRole, model.CreateRoleAdminOption);
                EditSysPrivs(model.User, "ALTER ANY ROLE", model.AlterAnyRole, model.AlterAnyRoleAdminOption);
                EditSysPrivs(model.User, "DROP ANY ROLE", model.DropAnyRole, model.DropAnyRoleAdminOption);
                EditSysPrivs(model.User, "GRANT ANY ROLE", model.GrantAnyRole, model.GrantAnyRoleAdminOption);
                EditSysPrivs(model.User, "CREATE SESSION", model.CreateSession, model.CreateSessionAdminOption);
                EditSysPrivs(model.User, "CREATE ANY TABLE", model.CreateAnyTable, model.CreateAnyTableAdminOption);
                EditSysPrivs(model.User, "ALTER ANY TABLE", model.AlterAnyTable, model.AlterAnyTableAdminOption);
                EditSysPrivs(model.User, "DROP ANY TABLE", model.DropAnyTable, model.DropAnyTableAdminOption);
                EditSysPrivs(model.User, "SELECT ANY TABLE", model.SelectAnyTable, model.SelectAnyTableAdminOption);
                EditSysPrivs(model.User, "DELETE ANY TABLE", model.DeleteAnyTable, model.DeleteAnyTableAdminOption);
                EditSysPrivs(model.User, "INSERT ANY TABLE", model.InsertAnyTable, model.InsertAnyTableAdminOption);
                EditSysPrivs(model.User, "UPDATE ANY TABLE", model.UpdateAnyTable, model.UpdateAnyTableAdminOption);
                EditSysPrivs(model.User, "CREATE TABLE", model.CreateTable, model.CreateTableAdminOption);
                EditSysPrivs(model.User, "CREATE USER", model.CreateUser, model.CreateUserAdminOption);
                EditSysPrivs(model.User, "ALTER USER", model.AlterUser, model.AlterUserAdminOption);
                EditSysPrivs(model.User, "DROP USER", model.DropUser, model.DropUserAdminOption);
            }
            return View(model);
        }
        // Ham xu ly quyen he thong
        private void EditSysPrivs(string user,string privsName, bool grant, bool adminOption)
        {
            try
            {
                using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    OracleCommand cmd = conn.CreateCommand();
                    if (grant && adminOption == false)

                    {
                        cmd.CommandText = $"GRANT {privsName} TO {user}";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"REVOKE {privsName} FROM {user}";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"GRANT {privsName} TO {user}";
                        cmd.ExecuteNonQuery();
                    }


                    if (grant && adminOption)
                    {
                            cmd.CommandText = $"GRANT {privsName} TO {user} WITH ADMIN OPTION ";
                            cmd.ExecuteNonQuery();
                    }
                                        
                    if(grant==false)
                    {
                        cmd.CommandText = $"REVOKE {privsName} FROM {user}";
                        cmd.ExecuteNonQuery();
                    }              
                }
            }
            catch
            {

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
                  
                }

            }

            return RedirectToAction(nameof(UserController.Index), "User");
        }
    }
}