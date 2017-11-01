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
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var listProfile = ole.Query<ProfileModel>("SELECT u.profile as PROFILE,count(u.username) AS NOOFUSER FROM dba_users u GROUP BY u.profile");
                return View(listProfile);
            }
        }


        [HttpGet]
        public IActionResult ViewProfile(string _profileName)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var profileResource = ole.Query<ProfileResource>("SELECT * FROM dba_profiles where Profile = '" + _profileName + "'");
                ViewBag.ProfileName = _profileName;
                return View(profileResource);
            }
        }

        [HttpGet]
        public IActionResult ViewUserInProfile(string _profileName)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var userInProfile = ole.Query<UserModel>("SELECT * FROM dba_users WHERE profile = '" + _profileName + "'");
                ViewBag.ProfileName = _profileName;
                return PartialView(userInProfile);
            }
                
        }

    }
}