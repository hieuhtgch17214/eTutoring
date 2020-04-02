using eTutoring.Models;
using eTutoring.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace eTutoring.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly AuthRepository _repo = new AuthRepository();

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.RegisterUser(userModel);
            var errors = GetErrorResult(result);

            if (errors != null)
            {
                return errors;
            }

            var successfulMessage = new
            {
                message = "Registration Completed"
            };

            return Ok(successfulMessage);
        }

        [Authorize]
        [HttpGet]
        [Route("personal-info")]
        public async Task<IHttpActionResult> GetPersonalInfo()
        {
            var id = User.Identity.GetUserId();
            var user = await _repo.FindUserById(id);
            var result = new
            {
                username = user.UserName,
                fullname = user.FullName,
                gender = user.Gender,
                birthday = user.Birthday,
                email = user.Email
            };
            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }
            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.Succeeded)
            {
                return null;
            }

            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (ModelState.IsValid)
            {
                return BadRequest();
            }

            return BadRequest(ModelState);
        }
    }
}