using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System.Linq;

namespace BK.UserManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Edit(string id)
        {
            using (var ole = new OracleConnection("Data Source=localhost/db11g;Persist Security Info=True;User ID=system;Password=Abc123;"))
            {
                var user = ole.Query<DbUser>("SELECT * FROM dba_users u WHERE u.USER_ID = :UserId", new { UserId = id })
                    .FirstOrDefault();
                return View(user);
            }
        }
    }
}