﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using BK.UserManagement.Web.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using BK.UserManagement.Web.Models.AccountViewModels;
using BK.UserManagement.Web.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BK.UserManagement.Web.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration config;
        public UserController(IConfiguration iconfig)
        {
            config = iconfig;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            //string con2 = String.Format(conn, username, password);
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var users = ole.Query<UserModel>("SELECT * FROM dba_users");
                return View(users);
            }

        }
        public IActionResult ListUserInfo()
        {
            //string con2 = String.Format(conn, username, password);
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var userInfo = ole.Query<UserInfoModel>("SELECT * FROM sys.dba_user_info");
                return View(userInfo);
            }

        }
        [HttpGet]
        public IActionResult AddUserInfo()
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                EditUserViewModel vmEditUser = new EditUserViewModel();
                //vmEditUser.DefaultTablespaceName = "SYSTEM";
                //vmEditUser.TemporaryTablespaceName = "TEMP";
                //vmEditUser.ProfileName = "DEFAULT";
                return View(vmEditUser);
            }
        }

        [HttpPost]
        public IActionResult AddUserInfo(EditUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
                {
                    //var user = ole.Query<UserModel>($"create user \"{model.Username}\" identified by \"{model.Password}\";grant create session to {model.Username};");
                    conn.Open();
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"
CREATE USER ""{model.Username.ToUpper()}"" IDENTIFIED BY ""{model.Password}""";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $@"GRANT CONNECT TO ""{ model.Username.ToUpper()}""";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $@"INSERT INTO sys.dba_user_info (USERNAME, FIRST_NAME, LAST_NAME, ADDRESS, PHONE, EMAIL) VALUES ('{ model.Username.ToUpper()}','{ model.FirstName}','{model.LastName}','{model.Address}','{model.Phone}','{model.Email}')";
                    cmd.ExecuteNonQuery();
                    return RedirectToAction(nameof(UserController.ListUserInfo), "User");
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult EditUserInfo(string id)
        {

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                EditUserViewModel vmEditUser = new EditUserViewModel();

                vmEditUser.User = ole.Query<UserModel>("SELECT * FROM dba_users u WHERE u.USERNAME = :Username", new { Username = id })
                    .FirstOrDefault();
                var userInfo = ole.Query<UserInfoModel>("SELECT * FROM sys.dba_user_info u WHERE u.USERNAME = :Username", new { Username = id.ToUpper() })
                     .FirstOrDefault();
                if (userInfo == null)
                {
                    ole.Query<int>($@"INSERT INTO sys.dba_user_info (USERNAME) VALUES ('{ id.ToUpper()}')");
                    userInfo = new UserInfoModel();
                }

                vmEditUser.FirstName = userInfo.FIRST_NAME;
                vmEditUser.LastName = userInfo.LAST_NAME;
                vmEditUser.Phone = userInfo.PHONE;
                vmEditUser.Email = userInfo.EMAIL;
                vmEditUser.Address = userInfo.ADDRESS;
                return View(vmEditUser);
            }

        }
        [HttpPost]
        public IActionResult EditUserInfo(EditUserViewModel model)
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
                            cmd.CommandText = $"ALTER USER {model.Username.ToUpper()} IDENTIFIED BY {model.Password}";
                            cmd.ExecuteNonQuery();
                        }

                        cmd.CommandText = $@"
UPDATE sys.dba_user_info
SET FIRST_NAME = '{model.FirstName}', 
LAST_NAME = '{model.LastName}', 
ADDRESS = '{model.Address}', 
PHONE = '{model.Phone}', 
EMAIL = '{model.Email}'
WHERE USERNAME = '{model.Username.ToUpper()}'";
                        cmd.ExecuteNonQuery();

                        //return RedirectToAction("EditUserInfo", "User", new { @id = model.Username });
                        return RedirectToAction(nameof(UserController.ListUserInfo), "User");
                        //return RedirectToLocal(returnUrl);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

            return RedirectToAction(nameof(UserController.Index), "User");
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                EditUserViewModel vmEditUser = new EditUserViewModel();

                vmEditUser.User = ole.Query<UserModel>("SELECT * FROM dba_users u WHERE u.USERNAME = :Username", new { Username = id })
                    .FirstOrDefault();
                var userInfo = ole.Query<UserInfoModel>("SELECT * FROM sys.dba_user_info u WHERE u.USERNAME = :Username", new { Username = id.ToUpper() })
                     .FirstOrDefault();
                if (userInfo == null)
                {
                    ole.Query<int>($@"INSERT INTO sys.dba_user_info (USERNAME) VALUES ('{ id.ToUpper()}')");
                    userInfo = new UserInfoModel();
                }

                vmEditUser.FirstName = userInfo.FIRST_NAME;
                vmEditUser.LastName = userInfo.LAST_NAME;
                vmEditUser.Phone = userInfo.PHONE;
                vmEditUser.Email = userInfo.EMAIL;
                vmEditUser.Address = userInfo.ADDRESS;
                var tablespaces = ole.Query<TablespaceModel>("SELECT TABLESPACE_NAME FROM SYS.DBA_TABLESPACES");
                vmEditUser.Tablespaces = tablespaces.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.TABLESPACE_NAME.ToString(),
                                      Value = x.TABLESPACE_NAME.ToString()
                                  });

                vmEditUser.DefaultTablespaceName = vmEditUser.User.DEFAULT_TABLESPACE;
                vmEditUser.TemporaryTablespaceName = vmEditUser.User.TEMPORARY_TABLESPACE;
                var profiles = ole.Query<ProfileModel>("SELECT DISTINCT PROFILE FROM SYS.DBA_PROFILES");
                vmEditUser.Profiles = profiles.Select(x =>
                                    new SelectListItem()
                                    {
                                        Text = x.PROFILE.ToString(),
                                        Value = x.PROFILE.ToString()
                                    });
                vmEditUser.ProfileName = vmEditUser.User.PROFILE;
                var accountStatusList = new List<SelectListItem>();
                accountStatusList.Add(new SelectListItem() { Text = "OPEN", Value = "0" });
                accountStatusList.Add(new SelectListItem() { Text = "PASSWORD EXPIRED", Value = "EXPIRED" });
                accountStatusList.Add(new SelectListItem() { Text = "LOCKED", Value = "LOCKED" });
                accountStatusList.Add(new SelectListItem() { Text = "EXPIRED & LOCKED", Value = "EXPIRED & LOCKED" });
                vmEditUser.AccoutStatusList = accountStatusList;
                vmEditUser.AccountStatus = vmEditUser.User.ACCOUNT_STATUS;
                vmEditUser.QuotaList = ole.Query<QuotaModel>($"SELECT TABLESPACE_NAME, BYTES, MAX_BYTES FROM SYS.DBA_TS_QUOTAS WHERE USERNAME = '{vmEditUser.User.USERNAME}'");
                return View(vmEditUser);
            }

        }
        [HttpPost]
        public IActionResult Edit(EditUserViewModel model)
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
                            cmd.CommandText = $"ALTER USER {model.Username.ToUpper()} IDENTIFIED BY {model.Password}";
                            cmd.ExecuteNonQuery();
                        }
                        cmd.CommandText = $@"ALTER USER {model.Username.ToUpper()} DEFAULT TABLESPACE {model.DefaultTablespaceName}";

                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"ALTER USER {model.Username.ToUpper()} TEMPORARY TABLESPACE {model.TemporaryTablespaceName}";

                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"ALTER USER {model.Username.ToUpper()} PROFILE {model.ProfileName}";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $@"
UPDATE sys.dba_user_info
SET FIRST_NAME = '{model.FirstName}', 
LAST_NAME = '{model.LastName}', 
ADDRESS = '{model.Address}', 
PHONE = '{model.Phone}', 
EMAIL = '{model.Email}'
WHERE USERNAME = '{model.Username.ToUpper()}'";
                        cmd.ExecuteNonQuery();
                        if (model.Quota != 0)
                        {
                            cmd.CommandText = $"ALTER USER {model.Username.ToUpper()} QUOTA {model.Quota}M ON {model.QuotaTablespace}";
                            cmd.ExecuteNonQuery();
                        }
                        return RedirectToAction("Edit", "User", new { @id = model.Username });
                        //return RedirectToAction(nameof(UserController.Index), "User");
                        //return RedirectToLocal(returnUrl);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

            return RedirectToAction(nameof(UserController.Index), "User");
        }
        [HttpGet]
        public IActionResult Add()
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                EditUserViewModel vmEditUser = new EditUserViewModel();

                //vmEditUser.User = ole.Query<UserModel>("SELECT * FROM dba_users u WHERE u.USERNAME = :Username", new { Username = id })
                //   .FirstOrDefault();
                var tablespaces = ole.Query<TablespaceModel>("SELECT TABLESPACE_NAME FROM SYS.DBA_TABLESPACES");
                vmEditUser.Tablespaces = tablespaces.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.TABLESPACE_NAME.ToString(),
                                      Value = x.TABLESPACE_NAME.ToString()
                                  });

                vmEditUser.DefaultTablespaceName = "SYSTEM";
                vmEditUser.TemporaryTablespaceName = "TEMP";
                var profiles = ole.Query<ProfileModel>("SELECT DISTINCT PROFILE FROM SYS.DBA_PROFILES");
                vmEditUser.Profiles = profiles.Select(x =>
                                    new SelectListItem()
                                    {
                                        Text = x.PROFILE.ToString(),
                                        Value = x.PROFILE.ToString()
                                    });
                vmEditUser.ProfileName = "DEFAULT";

                //                ole.Query<int>($@"INSERT INTO sys.dba_user_info (USERNAME,FIRST_NAME,LAST_NAME,ADDRESS,PHONE,EMAIL) 
                //VALUES ('{ vmEditUser.Username.ToUpper()}','{vmEditUser.FirstName}','{vmEditUser.LastName}','{vmEditUser.Address}','{vmEditUser.Phone}','{vmEditUser.Email}')");


                return View(vmEditUser);
            }
        }
        [HttpPost]
        public IActionResult Add(EditUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
                {
                    //var user = ole.Query<UserModel>($"create user \"{model.Username}\" identified by \"{model.Password}\";grant create session to {model.Username};");
                    conn.Open();
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"
CREATE USER ""{model.Username.ToUpper()}"" IDENTIFIED BY ""{model.Password}""
TEMPORARY TABLESPACE {model.TemporaryTablespaceName}
DEFAULT TABLESPACE {model.DefaultTablespaceName}
PROFILE  { model.ProfileName}";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $@"GRANT CONNECT TO ""{ model.Username.ToUpper()}""";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $@"INSERT INTO sys.dba_user_info (USERNAME, FIRST_NAME, LAST_NAME, ADDRESS, PHONE, EMAIL) VALUES ('{ model.Username.ToUpper()}','{ model.FirstName}','{model.LastName}','{model.Address}','{model.Phone}','{model.Email}')";
                    cmd.ExecuteNonQuery();

                    if (model.Quota != 0)
                    {
                        cmd.CommandText = $"ALTER USER {model.Username.ToUpper()} QUOTA {model.Quota}M ON {model.QuotaTablespace}";
                        cmd.ExecuteNonQuery();
                    }
                    return RedirectToAction(nameof(UserController.Index), "User");
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Unlock(String id)
        {

            
                using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    OracleCommand cmd = conn.CreateCommand();
                    //var user = ole.Query<UserModel>($"ALTER USER\"{model.Username.ToUpper()}\" IDENTIFIED BY \"{model.Password}\"");

                    cmd.CommandText =
                        $@"ALTER USER {id.ToUpper()} ACCOUNT UNLOCK";

                    cmd.ExecuteNonQuery();

                    return RedirectToAction("Edit", "User", new { @id = id });
                    //return RedirectToLocal(returnUrl);
                }
            


        }
        [HttpGet]
        public IActionResult Lock(String id)
        {
            using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                //var user = ole.Query<UserModel>($"ALTER USER\"{model.Username.ToUpper()}\" IDENTIFIED BY \"{model.Password}\"");

                cmd.CommandText =
                    $@"ALTER USER {id.ToUpper()} ACCOUNT LOCK";

                cmd.ExecuteNonQuery();

                return RedirectToAction("Edit", "User", new { @id = id });
            }
        }


        [HttpGet]
        public IActionResult Delete(string id)
        {
            using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var user = conn.Query<UserModel>($"DROP USER \"{id.ToUpper()}\" CASCADE");//id is USERNAME here
                //return RedirectToAction("Edit", "User", new { @id = id });
                return RedirectToAction(nameof(UserController.Index), "User");
                //return View();
            }
        }
        [HttpGet]
        public IActionResult DeleteUserInfo(string id)
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name;
            using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var username = conn.Query<string>($"SELECT USERNAME FROM sys.dba_users WHERE USERNAME= '{id.ToUpper()}'").FirstOrDefault();
                if (!String.IsNullOrWhiteSpace(username))
                {
                    conn.Query<string>($"DROP USER \"{id.ToUpper()}\"  CASCADE");
                }
                conn.Query<string>($"DELETE FROM sys.dba_user_info WHERE USERNAME='{id.ToUpper()}'");

                return RedirectToAction(nameof(UserController.ListUserInfo), "User");
                //return View();
            }
        }

    }
}
