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
            if (LoginUser(model.Username, model.Password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
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
            return View();
        }

        private bool LoginUser(string username, string password)
        {

            try
            {
                using (var con = new OracleConnection())
                {
                    con.ConnectionString = "Data Source=localhost/db11g;Persist Security Info=True;User ID=" + username + ";Password=" + password + ";";
                    con.Open();

                    return true;
                }
            }
            catch (Exception e)
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
    }
}
