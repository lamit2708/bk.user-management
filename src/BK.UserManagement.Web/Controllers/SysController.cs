using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using BK.UserManagement.Web.Models;
using Dapper;

namespace BK.UserManagement.Web.Controllers
{
    public class SysController : Controller
    {
        private readonly IConfiguration config;
        public SysController(IConfiguration iconfig)
        {
            config = iconfig;
        }
        public IActionResult Index()
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var sysPrivs = ole.Query<SysPrivsModel>("select * from sys.DBA_SYS_PRIVS");
                return View(sysPrivs);
            }
        }
        public IActionResult ListTablePrivs()
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var sysPrivs = ole.Query<TabPrivsModel>("select * from sys.DBA_TAB_PRIVS");
                return View(sysPrivs);
            }
        }
    }
}