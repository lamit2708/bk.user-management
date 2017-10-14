using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System.Linq;
using BK.UserManagement.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace BK.UserManagement.Web.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult LogOn(string id)
        {
            return View();

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //
        // GET: /Account/Login
        //[HttpGet]
        
        //public async Task<IActionResult> Login(string returnUrl = null)
        //{
        //    // Clear the existing external cookie to ensure a clean login process
        //    //await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);
        //    await HttpContext.Authentication.SignOutAsync("CookieAuthentication");

        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CookieAuthentication");
            return Redirect("/Account/Login");
        }
        [HttpPost]
        //public async Task<IActionResult> Login(LoginModel loginModel)
        public IActionResult Login(LoginModel loginModel)
        {
            if (LoginUser(loginModel.Username, loginModel.Password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Username)
            };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                HttpContext.Authentication.SignInAsync("CookieAuthentication", principal);
                
                //Just redirect to our index after logging in. 
                return Redirect("/");
            }
            return View();
        }
        [HttpPost]
        //public async Task<IActionResult> Login(LoginModel loginModel)
        public IActionResult Logon(LoginModel loginModel)
        {
            if (LoginUser(loginModel.Username, loginModel.Password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Username)
            };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                HttpContext.Authentication.SignInAsync("CookieAuthentication", principal);

                //Just redirect to our index after logging in. 
                return Redirect("/");
            }
            return View();
        }

        private bool LoginUser(string username, string password)
        {
            //As an example. This method would go to our data store and validate that the combination is correct. 
            //For now just return true. 
            return true;
        }
        public IActionResult Index()
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
    }
}
