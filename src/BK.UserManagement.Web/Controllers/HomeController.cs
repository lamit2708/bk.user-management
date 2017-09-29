using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;

namespace BK.UserManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            //using (var ole = new SqlConnection("asdfasdfsadfsdaf"))
            //{
            //    var user= ole.Query<object>("SELECT * FROM User WHERE Id = @Id", new { Id = 3 }).FirstOrDefault();
            //}
                return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
