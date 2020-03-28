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
        [HttpGet]
        public IHttpActionResult AmIAStudent()
        {
            var result = new
            {
                message = "You are a student"
            };

            return Ok(result);
        }

        [Authorize(Roles = "tutor")]
        [HttpGet]
        [Route("is-tutor")]
        public IHttpActionResult AmIATutor()
        {
            var result = new
            {
                message = "You are a tutor"
            };

            return Ok(result);
        }

        [Authorize(Roles = "staff")]
        [HttpGet]
        [Route("is-staff")]
        public IHttpActionResult AmIAStaff()
        {
            var result = new
            {
                message = "You are a staff member"
            };

            return Ok(result);
        }
    }
}