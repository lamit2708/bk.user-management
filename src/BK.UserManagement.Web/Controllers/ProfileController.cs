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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BK.UserManagement.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IConfiguration config;
        private string connString;
       
        public ProfileController(IConfiguration iconfig)
        {
            config = iconfig;
        }

        public bool isGranted(string _privilege, string _username)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                // SELECT user in SYS-PRIVILEGE
                
                var iSys = ole.Query("SELECT * FROM DBA_SYS_PRIVS WHERE GRANTEE = '" + _username.ToUpper() +"' AND PRIVILEGE LIKE '" + _privilege + "%'").Count();
                if (iSys > 0) return true;

                // SELECT user in ROLE-SYS-PRIVILEGE
                var iRoleSys = ole.Query(@"SELECT r.ROLE,r.PRIVILEGE, u.GRANTEE 
                                                            FROM sys.ROLE_SYS_PRIVS r
                                                            INNER JOIN sys.DBA_ROLE_PRIVS u
                                                            ON r.ROLE = u.GRANTED_ROLE WHERE GRANTEE = '" + _username.ToUpper() + "' AND PRIVILEGE LIKE '" + _privilege + "%'").Count();
                if (iRoleSys > 0) return true;

            }
            return false;
        }

        public bool profileExist(string _name)
        {
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var nProfile = ole.Query("select * from dba_profiles Where profile = '" + _name.ToUpper() + "'").Count();
                if (nProfile > 0) return true;
            }
            return false;
        }

        public IActionResult Index()
        {
            connString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication).Value;
            using (var ole = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                var listProfile = ole.Query<ProfileModel>("select p.profile as PROFILE,count(u.username) AS NOOFUSER from dba_users u RIGHT OUTER join(select DISTINCT profile from dba_profiles) p ON p.profile = u.profile GROUP BY p.profile");
                //ViewBag.Test = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication).Value;
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
                return View(userInProfile);
            }
                
        }

        public ActionResult EditResource(string pk, string value, string name)
        {
            using (var conn = new OracleConnection(config.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = $@"ALTER PROFILE {name} LIMIT {pk} {value}";

                cmd.ExecuteNonQuery();
                return new StatusCodeResult(200);
            } 
        }

        [HttpGet]
        public IActionResult AddNewProfile()
        {
            //check user current login is Granted *** need update ***
            if (isGranted("CREATE PROFILE", HttpContext.User.Identity.Name))
            {
                return View();
            }
            else
            {
                //"You don't have permission to this action. Please contact administrator.";
                return RedirectToAction("NoPermit"); // 
            }

            
        }



        [HttpPost]
        public IActionResult AddNewProfile(ProfileModel pm)
        {
            if (ModelState.IsValid)
            {
                //check user current login is Granted *** need update ***
                ViewBag.Message = "You don't have permission to this action. Please contact administrator.";

                if (isGranted("CREATE PROFILE", HttpContext.User.Identity.Name))
                {
                    try
                    {

                        //check if profile exist
                        if (profileExist(pm.PROFILE))
                        {
                            ViewBag.Message = "Profile already exist. Please try again";
                            return View();
                        }
                        else
                        {
                            connString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication).Value;
                            using (var conn = new OracleConnection(connString))
                            {
                                conn.Open();
                                OracleCommand cmd = conn.CreateCommand();
                                cmd.CommandText = $@"CREATE PROFILE {pm.PROFILE.ToUpper()} LIMIT";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    //"You don't have permission to this action. Please contact administrator.";
                    return RedirectToAction("NoPermit"); // 
                }

                

            }

            return RedirectToAction(nameof(UserController.Index), "Profile");
        }

        [HttpGet]
        public IActionResult NoPermit()
        {
               return View();
        }

        [HttpGet]
        public IActionResult DeleteProfile(string _profileName)
        {
            //check user current login is Granted *** need update ***
            if (isGranted("DROP PROFILE", HttpContext.User.Identity.Name))
            {
                ViewBag.ProfileName = _profileName;
                return View();
            }
            else
            {
                return RedirectToAction("NoPermit"); // 
            }
        }

        [HttpPost]
        public IActionResult Delete(string _profileName)
        {
            //check user current login is Granted *** need update ***
            if (isGranted("DROP PROFILE", HttpContext.User.Identity.Name))
            {
                try
                {
                    connString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication).Value;
                    using (var conn = new OracleConnection(connString))
                    {
                        conn.Open();
                        OracleCommand cmd = conn.CreateCommand();
                        cmd.CommandText = $@"DROP PROFILE {_profileName} CASCADE";
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }


                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NoPermit"); // 
            }
            
        }

        [HttpGet]
        public IActionResult ResetProfile(string _profileName)
        {
            ViewBag.Profile = _profileName;
            return PartialView();

        }

        [HttpGet]
        public IActionResult CopyProfile(string _profileSrc)
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult CopyProfile(string _profileSrc,string _profileDest)
        {

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Edit(string _profileName,string _resourceName)
        {
            if (isGranted("ALTER PROFILE", HttpContext.User.Identity.Name))
            {
                connString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication).Value;
                using (var conn = new OracleConnection(connString))
                {
                    var profileResource = conn.Query<ProfileResource>("SELECT * FROM dba_profiles where Profile = '" + _profileName + "' AND RESOURCE_NAME = '" + _resourceName + "'").FirstOrDefault();
                    ViewBag.ProfileName = _profileName;
                    return View(profileResource);
                }
            }
            else
            {
                return RedirectToAction("NoPermit"); // 
            }
            
        }

        

    }
}