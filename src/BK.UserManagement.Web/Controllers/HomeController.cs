using Microsoft.AspNetCore.Mvc;
using Dapper;
using Oracle.ManagedDataAccess.Client;

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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            using (var ole = new OracleConnection("Data Source=localhost/db11g;Persist Security Info=True;User ID=system;Password=Abc123;"))
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
