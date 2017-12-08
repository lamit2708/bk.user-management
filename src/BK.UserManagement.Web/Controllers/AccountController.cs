using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System.Linq;
using BK.UserManagement.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using BK.UserManagement.Web.Models.AccountViewModels;
using System.Data.OleDb;
using System;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authorization;
using BK.UserManagement.Web.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BK.UserManagement.Web.Controllers
{
    
    public class AccountController : Controller
    {

        private readonly IConfiguration config;
        public AccountController(IConfiguration iconfig)
        {
            config = iconfig;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync("CookieAuthentication");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        //public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                if (LoginUser(model.Server + "/" + model.Sid, model.Username, model.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Authentication, String.Format(config.GetConnectionString("UserConnection"), model.Server + "/" + model.Sid, model.Username, model.Password)),

                    };

                    var userIdentity = new ClaimsIdentity(claims, "login");
                    if (model.RememberMe)
                    {
                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.Authentication.SignInAsync("CookieAuthentication",
                            principal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true
                            });
                    }
                    else
                    {
                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.Authentication.SignInAsync("CookieAuthentication",
                            principal,
                            new AuthenticationProperties
                            {
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
                            });
                    }
                    return RedirectToLocal(returnUrl);
                    //Just redirect to our index after logging in. 
                    //return Redirect("/");
                }
            }
            return View();
        }

        private bool LoginUser(string dataSource, string username, string password)
        {

            try
            {

                var con = new OracleConnection();
                con.ConnectionString = String.Format(config.GetConnectionString("UserConnection"), dataSource, username, password);
                con.Open();
                con.Close();
                con.Dispose();
                return true;
                
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return false;
            }

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CookieAuthentication");
            return Redirect("/Account/Login");
        }
        private IActionResult Index()
        {
            var loggedInUser = HttpContext.User;
            var loggedInUserName = loggedInUser.Identity.Name; // This is our username we set earlier in the claims. 
            var loggedInUserName2 = loggedInUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value; //Another way to get the name or any other claim we set. 
            return View();
        }
        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        private IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        private IActionResult Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
                {

                    var user = ole.Query<UserModel>($"create user \"{model.Username}\" identified by \"{model.Password}\"");
                    return RedirectToAction(nameof(UserController.Index), "User");

                }

            }


            return View(model);
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //}
        [Authorize]
        [HttpGet]
        public IActionResult Logon()
        {
            var loggedInUser = HttpContext.User;
            string username = loggedInUser.Identity.Name;
            
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                EditUserViewModel vmEditUser = new EditUserViewModel();

                vmEditUser.User = ole.Query<UserModel>("SELECT * FROM sys.dba_users WHERE USERNAME = :Username", new { Username = username.ToUpper() })
                    .FirstOrDefault();
                var userInfo = ole.Query<UserInfoModel>("SELECT * FROM sys.dba_user_info u WHERE u.USERNAME = :Username", new { Username = username.ToUpper() })
                    .FirstOrDefault();
                if(userInfo == null)
                {
                    ole.Query<int>($@"INSERT INTO sys.dba_user_info (USERNAME) VALUES ('{ username.ToUpper()}')");
                    userInfo = new UserInfoModel();
                }
                vmEditUser.FirstName = userInfo.FIRST_NAME;
                vmEditUser.LastName = userInfo.LAST_NAME;
                vmEditUser.Phone = userInfo.PHONE;
                vmEditUser.Email = userInfo.EMAIL;
                vmEditUser.Address = userInfo.ADDRESS;
                

                vmEditUser.DefaultTablespaceName = vmEditUser.User.DEFAULT_TABLESPACE;
                vmEditUser.TemporaryTablespaceName = vmEditUser.User.TEMPORARY_TABLESPACE;
                
                vmEditUser.ProfileName = vmEditUser.User.PROFILE;
                vmEditUser.AccountStatus = vmEditUser.User.ACCOUNT_STATUS;
                vmEditUser.QuotaList = ole.Query<QuotaModel>($"SELECT TABLESPACE_NAME, BYTES, MAX_BYTES FROM SYS.DBA_TS_QUOTAS WHERE USERNAME = '{vmEditUser.User.USERNAME}'");
                return View(vmEditUser);
            }

        }
        [HttpPost]
        public IActionResult Logon(EditUserViewModel model)
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

                        //return View(vmEditUser);
                        //return RedirectToAction("Edit", "User", new { @id = model.Username });
                        //return RedirectToAction(nameof(AccountController.Logon), "Account");
                        //return RedirectToLocal(returnUrl);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

            return RedirectToAction(nameof(AccountController.Logon), "Account");
        }
    }
}
