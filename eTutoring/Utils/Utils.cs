using eTutoring.Models;
using eTutoring.Models.DTO;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Utils
{
    public static class Utils
    {
        public static ApplicationUser ToApplicationUser(this UserFormModel userModel) => new ApplicationUser
        {
            UserName = userModel.UserName,
            Email = userModel.Email,
            FullName = userModel.FullName,
            Gender = userModel.Gender,
            Birthday = userModel.Birthday
        };

        public static UserResponseModelDto ToUserResponseModel(this ApplicationUser user) => new UserResponseModelDto
        {
            ID = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            Gender = user.Gender,
            Birthday = user.Birthday
        };
    }
}