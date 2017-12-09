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

                //var listPriv = ole.Query($"select * from SESSION_PRIVS");
                //var sl = listPriv.Select(x =>
                //                    new SelectListItem()
                //                    {
                //                        Text = x.PRIVILEGE.ToString(),
                //                        Value = x.PRIVILEGE.ToString()
                //                    });
                //ViewBag.ListPriv = sl.ToList();


                 ViewBag.ListPriv = new SelectList(new[] { "CREATE PROFILE", "ALTER PROFILE", "DROP PROFILE", "CREATE ROLE", "ALTER ANY ROLE", "DROP ANY ROLE", "GRANT ANY ROLE", "CREATE SESSION"
                 , "CREATE ANY TABLE", "ALTER ANY TABLE", "DROP ANY TABLE", "SELECT ANY TABLE", "DELETE ANY TABLE", "INSERT ANY TABLE", "UPDATE ANY TABLE", "CREATE TABLE", "CREATE USER", "ALTER USER", "DROP USER" }, "");
                ViewBag.ListAdmin = new SelectList(new[] { "YES", "NO"}, "YES");
                ViewBag.ListType = new SelectList(new[] { "GRANT", "REVOKE" }, "GRANT");
                ViewBag.Username = id;
                var listSysPriv = ole.Query<SysPrivsModel>($"select * from sys.DBA_SYS_PRIVS WHERE GRANTEE = '{id}'");
                return View(listSysPriv);
            }

        }


        [HttpPost]
        public IActionResult GrantSysPrivs(string _username,string ListPriv,string ListAdmin,string ListType)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                if (ListType == "GRANT")
                {

                    var iExist = ole.Query($@"SELECT * FROM DBA_SYS_PRIVS WHERE GRANTEE = '{_username}' AND PRIVILEGE LIKE '{ListPriv}%'").Count();
                    if (iExist > 0)
                    {
                        ole.Query<string>($@"REVOKE {ListPriv} FROM {_username}");
                    }

                    
                    //ole.Query<string>($@"REVOKE {ListPriv} FROM {_username}");

                    if (ListAdmin == "YES")
                    {
                        ole.Query<string>($@"{ListType} {ListPriv} TO {_username} WITH ADMIN OPTION");
                    }
                    else
                    {
                        ole.Query<string>($@"{ListType} {ListPriv} TO {_username}");
                    }
                    
                }
                else if(ListType == "REVOKE")
                {
                    var iExist = ole.Query($@"SELECT * FROM DBA_SYS_PRIVS WHERE GRANTEE = '{_username}' AND PRIVILEGE LIKE '{ListPriv}%'").Count();
                    if (iExist > 0)
                    {
                        ole.Query<string>($@"{ListType} {ListPriv} FROM {_username}");
                    }

                    
                }
                

                return RedirectToAction("GrantSysPrivs",new { id = _username });
            }

        }


    }
}