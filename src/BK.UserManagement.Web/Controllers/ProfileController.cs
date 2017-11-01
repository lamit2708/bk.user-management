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

        public ActionResult EditResource(string pk, string value, string name)
        {
            using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = $@"ALTER PROFILE {pk} LIMIT {pk} {value}";

                cmd.ExecuteNonQuery();
                return new StatusCodeResult(200);
            }



            //var dev = db.tblDevice_Model.Find(pk);
            //if (dev != null)
            //{
            //    dev.ModelName = value;
            //    db.SaveChanges();
            //    return new HttpStatusCodeResult(HttpStatusCode.OK);
            //}
            //else
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            
        }

    }
}