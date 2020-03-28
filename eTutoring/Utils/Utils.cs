using eTutoring.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Utils
{
    public static class Utils
    {
        public static ApplicationUser ToApplicationUser(this UserModel userModel) => new ApplicationUser
        {
            UserName = userModel.UserName,
            Email = userModel.Email,
            FullName = userModel.FullName
        };
    }
}