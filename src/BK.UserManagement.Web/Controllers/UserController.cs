using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using BK.UserManagement.Web.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BK.UserManagement.Web.Controllers
{
    [Authorize]
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
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var users = ole.Query<DbUser>("SELECT * FROM dba_users");
                return View(users);
            }

        }
        public IActionResult Edit(string id)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var user = ole.Query<DbUser>("SELECT * FROM dba_users u WHERE u.USER_ID = :UserId", new { UserId = id })
                    .FirstOrDefault();
                //var cmd = conn.CreateCommand();
                //cmd.CommandText =
                //  "SELECT * FROM dba_users u WHERE u.USER_ID = :UserId";
                //var p = cmd.Parameters;
                //p.Add("UserId", id);
                //cmd.BindByName = true;
                //cmd.ExecuteNonQuery();
                return View(user);
            }
        }
        public IActionResult Add(string id)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var user = ole.Query<DbUser>("create user userID = :UserId", new { UserId = id })
                    .FirstOrDefault();
                //var cmd = conn.CreateCommand();
                //cmd.CommandText =
                //  "SELECT * FROM dba_users u WHERE u.USER_ID = :UserId";
                //var p = cmd.Parameters;
                //p.Add("UserId", id);
                //cmd.BindByName = true;
                //cmd.ExecuteNonQuery();
                return View(user);
            }
        }
        //private Task<ApplicationUser> GetCurrentUserAsync()
        //{
        //    return _userManager.GetUserAsync(HttpContext.User);
        //}



    }
}
