using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace eTutoring.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthTestController : ApiController
    {
        [Authorize(Roles = "student")]
        [Route("is-student")]
        public IHttpActionResult AmIAStudent()
        {
            var result = new
            {
                message = "You are a student"
            };

            return Ok(result);
        }
    }
}