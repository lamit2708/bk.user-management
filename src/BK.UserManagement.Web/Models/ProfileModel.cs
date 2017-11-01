using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using BK.UserManagement.Web.Models;
using Dapper;
using BK.UserManagement.Web.Models.RoleViewModels;
namespace BK.UserManagement.Web.Models
{
    public class ProfileModel
    {
        
        public string PROFILE { get; set; }
        public string NOOFUSER { get; set; }

    }
}
