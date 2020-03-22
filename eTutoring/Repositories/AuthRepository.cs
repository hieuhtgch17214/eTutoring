using eTutoring.DbContext;
using eTutoring.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eTutoring.Repositories
{
    public class AuthRepository : IDisposable
    {
        private readonly AuthContext _authContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository()
        {
            _authContext = new AuthContext();
            var userStore = new UserStore<ApplicationUser>(_authContext);
            _userManager = new UserManager<ApplicationUser>(userStore);
        }

        public Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var appUser = new ApplicationUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
                FullName = userModel.FullName
            };
            return _userManager.CreateAsync(appUser, userModel.Password);
        }

        public Task<ApplicationUser> FindUser(string username, string password)
        {
            return _userManager.FindAsync(username, password);
        }

        public void Dispose()
        {
            _authContext.Dispose();
            _userManager.Dispose();
        }
    }
}