using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BK.UserManagement.Web.Controllers
{
    public class PrivilegeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GrantSysPrivs()
        {
            return View();
        }

    }
}