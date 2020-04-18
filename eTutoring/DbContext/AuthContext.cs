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
        public AuthContext() : base("eTutoringContext")
        {

        }

        public DbSet<TutorAllocationModel> TutorAllocations { get; set; }
        public DbSet<BlogPostModel> BlogPosts { get; set; }
        public DbSet<FileUploadModel> FileUploads { get; set; }
    }
}