using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace BK.UserManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration config;
        public AccountController(IConfiguration iconfig)
        {
            config = iconfig;
        }
        public IActionResult Edit(string id)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var user = ole.Query<DbUser>("SELECT * FROM dba_users u WHERE u.USER_ID = :UserId", new { UserId = id })
                    .FirstOrDefault();
                return View(user);
            }
        }

        public IActionResult LogOn(string id)
        {
            return View();

        }
    }
}
