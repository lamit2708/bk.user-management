using Microsoft.AspNetCore.Mvc;
using Dapper;
using Oracle.ManagedDataAccess.Client;
//using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System;
using BK.UserManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using BK.UserManagement.Web.Models.DashboardViewModels;
using System.Linq;
using System.Security.Claims;

namespace BK.UserManagement.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IConfiguration config;
        public HomeController(IConfiguration iconfig)
        {
            config = iconfig;
        }
        public IActionResult Index()
        {
            var vmDashboard = new DashboardViewModel();
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                vmDashboard.NumOfUsers = ole.Query<int>("SELECT COUNT(*) FROM sys.dba_users").FirstOrDefault();
                vmDashboard.NumOfRoles = ole.Query<int>("SELECT COUNT(*) FROM sys.dba_roles").FirstOrDefault();
                vmDashboard.NumOfProfiles = ole.Query<int>("SELECT COUNT(DISTINCT PROFILE) FROM sys.dba_profiles").FirstOrDefault();
            }
            //var connString1 = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication).Value;
            var connString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication).Value;
            using (var ole = new OracleConnection(connString))
            {
                vmDashboard.NumOfSessionPrivs = ole.Query<int>("SELECT COUNT(*) FROM sys.session_privs").FirstOrDefault();
                vmDashboard.NumOfSessionRoles = ole.Query<int>("SELECT COUNT(*) FROM sys.session_roles").FirstOrDefault();
            }
            return View(vmDashboard);

        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult About()
        {
            //_iconfiguration.GetSection("Data").GetSection("ConnectionString").Value;
            //_iconfiguration.GetValue<string>("ConnectionStrings:DefaultConnection")
            //config.GetSection("ConnectionStrings")["DefaultConnection"]

            ViewData["Message"] = "Your application description page.";
            
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var users = ole.Query<UserModel>("SELECT * FROM dba_users");
                return View(users);
            }
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            
                return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
