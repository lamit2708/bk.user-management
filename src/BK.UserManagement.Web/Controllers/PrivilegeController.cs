using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BK.UserManagement.Web.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BK.UserManagement.Web.Controllers
{
    public class PrivilegeController : Controller
    {

        private readonly IConfiguration config;
        private string connString;

        public PrivilegeController(IConfiguration iconfig)
        {
            config = iconfig;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GrantSysPrivs(string id)
        {



            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {

                var listPriv = ole.Query($"select * from SESSION_PRIVS");
                var sl = listPriv.Select(x =>
                                    new SelectListItem()
                                    {
                                        Text = x.PRIVILEGE.ToString(),
                                        Value = x.PRIVILEGE.ToString()
                                    });
                ViewBag.ListPriv = sl.ToList();

                ViewBag.ListAdmin = new SelectList(new[] { "YES", "NO"}, "YES");
                ViewBag.ListType = new SelectList(new[] { "GRANT", "REVOKE" }, "GRANT");

                var listSysPriv = ole.Query<SysPrivsModel>($"select * from sys.DBA_SYS_PRIVS WHERE GRANTEE = '{id}'");
                return View(listSysPriv);
            }


               
        }

    }
}