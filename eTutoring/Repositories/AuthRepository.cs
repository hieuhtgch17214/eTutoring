using eTutoring.DbContext;
using eTutoring.Models;
using eTutoring.Utils;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository()
        {
            _authContext = new AuthContext();
            var userStore = new UserStore<ApplicationUser>(_authContext);
            var roleStore = new RoleStore<IdentityRole>(_authContext);
            _userManager = new UserManager<ApplicationUser>(userStore);
            _roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            await AddRolesIfNotExists(userModel.Role);
            var appUser = userModel.ToApplicationUser();

            // Create user
            await _userManager.CreateAsync(appUser, userModel.Password);

            // add user to role table
            var createdUser = await _userManager.FindByNameAsync(userModel.UserName);
            return await _userManager.AddToRoleAsync(createdUser.Id, userModel.Role);
        }

        public Task<ApplicationUser> FindUser(string username, string password)
        {
            return _userManager.FindAsync(username, password);
        }

        public Task<ApplicationUser> FindUserById(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<IList<string>> GetUserRolesById(string userId)
        {
            return _userManager.GetRolesAsync(userId);
        }

        public void Dispose()
        {
            _authContext.Dispose();
            _userManager.Dispose();
        }

        private async Task AddRolesIfNotExists(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists) return;
            var role = new IdentityRole
            {
                Name = roleName
            };
            await _roleManager.CreateAsync(role);
        }
    }
}