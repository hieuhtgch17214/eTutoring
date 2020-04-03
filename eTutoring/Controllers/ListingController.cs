using eTutoring.Repositories;
using eTutoring.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace eTutoring.Controllers
{
    [RoutePrefix("api/list")]
    [Authorize(Roles = "staff")]
    public class ListingController : ApiController
    {
        private readonly AuthRepository _repo = new AuthRepository();

        [HttpGet]
        [Route("tutors")]
        public async Task<IHttpActionResult> ListAllTutors()
        {
            var tutors = await _repo.AllTutors();
            var response = tutors.Select(tutor => tutor.ToUserResponseModel());
            return Ok(response);
        }

        [HttpGet]
        [Route("students")]
        public async Task<IHttpActionResult> ListAllStudents()
        {
            var tutors = await _repo.AllStudents();
            var response = tutors.Select(tutor => tutor.ToUserResponseModel());
            return Ok(response);
        }
    }
}
