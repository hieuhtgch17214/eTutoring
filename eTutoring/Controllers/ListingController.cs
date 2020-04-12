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
        public IHttpActionResult ListAllTutors()
        {
            var tutors = _repo.AllTutors();
            var response = tutors.Select(tutor => tutor.ToUserResponseModel());
            return Ok(response);
        }

        [HttpGet]
        [Route("tutor")]
        public async Task<IHttpActionResult> ListAllTutorsWithIds(string ids)
        {
            if (ids == null) return BadRequest();

            var idArray = ids.Split(',');
            var tutors = await _repo.FindTutorsByIds(idArray);
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

        [HttpGet]
        [Route("student")]
        public async Task<IHttpActionResult> ListAllStudentsWithIds(string ids)
        {
            if (ids == null) return BadRequest();

            var idArray = ids.Split(',');
            var tutors = await _repo.FindStudentsByIds(idArray);
            var response = tutors.Select(tutor => tutor.ToUserResponseModel());
            return Ok(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
