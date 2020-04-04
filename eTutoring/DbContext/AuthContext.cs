using eTutoring.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eTutoring.DbContext
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext() : base("AuthContext")
        {

        }

        public DbSet<TutorAllocationModel> TutorAllocations { get; set; }
    }
}