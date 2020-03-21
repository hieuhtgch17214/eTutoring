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
        private readonly UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _authContext = new AuthContext();
            var userStore = new UserStore<IdentityUser>(_authContext);
            _userManager = new UserManager<IdentityUser>(userStore);
        }

        public Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            return _userManager.CreateAsync(user, userModel.Password);
        }

        public Task<IdentityUser> FindUser(string username, string password)
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