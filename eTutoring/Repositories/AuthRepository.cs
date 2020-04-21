using eTutoring.DbContext;
using eTutoring.Models;
using eTutoring.Models.DTO;
using eTutoring.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<IdentityResult> RegisterUser(UserFormModel userModel)
        {
            await AddRoleIfNotExists(userModel.Role);
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

        public async Task<UserResponseModelDto> GetUserResponse(string userId)
        {
            var userTask = _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(userId);
            return (await userTask).ToUserResponseModel(roles.First());
        }

        public IEnumerable<ApplicationUser> AllTutors()
        {
            var role = _roleManager.FindByName("tutor");
            var user = from oneuser in _userManager.Users
                where oneuser.Roles.Any(r => r.RoleId == role.Id)
                select oneuser;
            return user.AsEnumerable();
        }

        public async Task<IList<ApplicationUser>> FindTutorsByIds(string[] ids)
        {
            var role = await _roleManager.FindByNameAsync("tutor");
            var user = from oneuser in _userManager.Users
                       where oneuser.Roles.Any(r => r.RoleId == role.Id)
                            && ids.Contains(oneuser.Id)
                       select oneuser;
            return await user.ToListAsync();
        }

        public async Task<IList<ApplicationUser>> AllStudents()
        {
            var role = await _roleManager.FindByNameAsync("student");
            var user = from oneuser in _userManager.Users
                       where oneuser.Roles.Any(r => r.RoleId == role.Id)
                       select oneuser;
            return await user.ToListAsync();
        }

        public async Task<IList<ApplicationUser>> FindStudentsByIds(string[] ids)
        {
            var role = await _roleManager.FindByNameAsync("student");
            var user = from oneuser in _userManager.Users
                       where oneuser.Roles.Any(r => r.RoleId == role.Id)
                            && ids.Contains(oneuser.Id)
                       select oneuser;
            return await user.ToListAsync();
        }

        public void Dispose()
        {
            _authContext.Dispose();
            _roleManager.Dispose();
            _userManager.Dispose();
        }

        private async Task AddRoleIfNotExists(string roleName)
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