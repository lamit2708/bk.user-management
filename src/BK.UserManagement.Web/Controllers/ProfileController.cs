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
                var listProfile = ole.Query<ProfileModel>("SELECT * FROM dba_profiles");
                return View(listProfile);
            }
        }
    }
}