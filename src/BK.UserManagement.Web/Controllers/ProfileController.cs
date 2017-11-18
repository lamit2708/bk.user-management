using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using BK.UserManagement.Web.Models;
using Dapper;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace BK.UserManagement.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IConfiguration config;
        public ProfileController(IConfiguration iconfig)
        {
            config = iconfig;
        }



        public IActionResult Index()
        {
            using (var ole = new OracleConnection(config.GetConnectionString("PhucConnection")))
            {
                var listProfile = ole.Query<ProfileModel>("select p.profile as PROFILE,count(u.username) AS NOOFUSER from dba_users u RIGHT OUTER join(select DISTINCT profile from dba_profiles) p ON p.profile = u.profile GROUP BY p.profile");
                return View(listProfile);
            }
        }


        [HttpGet]
        public IActionResult ViewProfile(string _profileName)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("PhucConnection")))
            {
                var profileResource = ole.Query<ProfileResource>("SELECT * FROM dba_profiles where Profile = '" + _profileName + "'");
                ViewBag.ProfileName = _profileName;
                return View(profileResource);
            }
        }

        [HttpGet]
        public IActionResult ViewUserInProfile(string _profileName)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("PhucConnection")))
            {
                var userInProfile = ole.Query<UserModel>("SELECT * FROM dba_users WHERE profile = '" + _profileName + "'");
                ViewBag.ProfileName = _profileName;
                return PartialView(userInProfile);
            }
                
        }

        public ActionResult EditResource(string pk, string value, string name)
        {
            using (var conn = new OracleConnection(config.GetConnectionString("PhucConnection")))
            {
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = $@"ALTER PROFILE {pk} LIMIT {pk} {value}";

                cmd.ExecuteNonQuery();
                return new StatusCodeResult(200);
            } 
        }

        [HttpGet]
        public IActionResult AddNewProfile()
        {
            return View();
        }



    }
}