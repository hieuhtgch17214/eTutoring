using eTutoring.DbContext;
using eTutoring.Models;
using eTutoring.Models.DTO;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public static IEnumerable<AllocationResponseModel> ToAllocationResponses(this AuthContext context)
        {
            var allocations = from allocation in context.TutorAllocations
                                   join tutor in context.Users on allocation.TutorId equals tutor.Id
                                   join student in context.Users on allocation.StudentId equals student.Id
                                   select new { tutor, student };
            var allocationGroup = from allocation in allocations
                                  group allocation by allocation.tutor into one_group
                                  select one_group;

            var result = new List<AllocationResponseModel>();
            foreach (var oneGroup in allocationGroup)
            {
                var students = oneGroup.Select(data => data.student.ToUserResponseModel());

                result.Add(new AllocationResponseModel
                {
                    Tutor = oneGroup.Key.ToUserResponseModel(),
                    Students = students
                });
            }

            return result;
        }
    }
}