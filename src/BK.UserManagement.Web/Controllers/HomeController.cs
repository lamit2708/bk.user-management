using Microsoft.AspNetCore.Mvc;
using Dapper;
using Oracle.ManagedDataAccess.Client;
//using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System;

namespace BK.UserManagement.Web.Controllers
{
    public class DbUser
    {
        public string USER_ID { get; set; }
        public string USERNAME { get; set; }
        public string ACCOUNT_STATUS { get; set; }
        public string LOCK_DATE { get; set; }
        public string EXPIRE_DATE { get; set; }
        // TODO
    }

    public class HomeController : Controller
    {
        IConfiguration _iconfiguration;
        public HomeController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            //_iconfiguration.GetSection("Data").GetSection("ConnectionString").Value;
            //_iconfiguration.GetValue<string>("ConnectionStrings:DefaultConnection")
            ViewData["Message"] = "Your application description page.";
            
            using (var ole = new OracleConnection(_iconfiguration.GetSection("ConnectionStrings")["DefaultConnection"]))
            {
                var users = ole.Query<DbUser>("SELECT * FROM dba_users");
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
