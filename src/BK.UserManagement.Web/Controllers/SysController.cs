using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using BK.UserManagement.Web.Models;
using Dapper;
using BK.UserManagement.Web.Models.DataTableViewModels;
using BK.UserManagement.Web.Models.RoleViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult ListRolePrivsUser()
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var sysPrivs = ole.Query<ListRolePrivsUserModel>(@"SELECT r.ROLE,r.PRIVILEGE, u.GRANTEE 
                                                            FROM sys.ROLE_SYS_PRIVS r
                                                            INNER JOIN sys.DBA_ROLE_PRIVS u
                                                            ON r.ROLE = u.GRANTED_ROLE");
                return View(sysPrivs);
            }

        }
        [HttpPost]
        public IActionResult ListTablePrivsAjax(DataTableParamViewModel param)
        {
            var requestFormData = Request.Form;
            var search = param.search.value;
            var searchCondition = !string.IsNullOrWhiteSpace(search)
                ? $@"WHERE GRANTEE LIKE '%{search}%' 
 OR OWNER LIKE '%{search}%'
 OR TABLE_NAME LIKE '%{search}%'
 OR GRANTOR LIKE '%{search}%'
 OR PRIVILEGE LIKE '%{search}%' "
                : string.Empty;

            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var total = ole.Query<int>($@"
select COUNT(*)
from sys.DBA_TAB_PRIVS
 {searchCondition}").FirstOrDefault();

               
                var listItems = ole.Query<TabPrivsModel>($@"
select b.*
 from (select a.*, ROWNUM rnum
       from ( select * 
                from sys.DBA_TAB_PRIVS
                {searchCondition}
            ) a
       where ROWNUM <= {(param.start + param.length)}) b
 where b.rnum > {param.start}");
                return Json(new
                {
                    Data = listItems,
                    Draw = requestFormData["draw"],
                    RecordsFiltered = total,
                    RecordsTotal = total
                });
            }
            
            
            
            
        }
        [HttpGet]
        public IActionResult GrantRoleToUser(string id, [FromQuery] string username)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                
                var vmGrantRoleToUser = new GrantRoleToUserViewModel();
                vmGrantRoleToUser.GrantedRole = id;
                var users = ole.Query<string>(@"SELECT DISTINCT USERNAME FROM sys.dba_users");
                vmGrantRoleToUser.Users = users.Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x,
                        Value = x
                    };
                });
                vmGrantRoleToUser.Username = username;
                return View(vmGrantRoleToUser);
            }

        }

        [HttpPost]
        public IActionResult GrantRoleToUser(GrantRoleToUserViewModel model)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                ole.Query<string>($@"GRANT {model.GrantedRole} TO {model.Username}");

                return RedirectToAction(nameof(SysController.ListRolePrivsUser), "Sys");
            }

        }

        [HttpGet]
        public IActionResult RevokeRoleFromUser(string id, [FromQuery] string username)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {

                var vmGrantRoleToUser = new GrantRoleToUserViewModel();
                vmGrantRoleToUser.GrantedRole = id;
                var users = ole.Query<string>(@"SELECT DISTINCT USERNAME FROM sys.dba_users");
                vmGrantRoleToUser.Users = users.Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x,
                        Value = x
                    };
                });
                vmGrantRoleToUser.Username = username;
                return View(vmGrantRoleToUser);
            }

        }

        [HttpPost]
        public IActionResult RevokeRoleFromUser(GrantRoleToUserViewModel model)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                ole.Query<string>($@"REVOKE {model.GrantedRole} FROM {model.Username}");
                return RedirectToAction(nameof(SysController.ListRolePrivsUser), "Sys");
            }

        }
    }

}