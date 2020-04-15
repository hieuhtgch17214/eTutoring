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
        private readonly AuthRepository _authRepo = new AuthRepository();
        private readonly TutorAllocationRepository _allocationRepo = new TutorAllocationRepository();

        [HttpGet]
        [Route("tutors")]
        public IHttpActionResult ListAllTutors()
        {
            var tutors = _authRepo.AllTutors();
            var response = tutors.Select(tutor => tutor.ToUserResponseModel());
            return Ok(response);
        }

        [HttpGet]
        [Route("tutor")]
        public IHttpActionResult ListAllTutorsWithIds(string ids)
        {
            if (ids == null || ids.Length == 0) return BadRequest();

            var idArray = ids.Split(',');
            var allocations = _allocationRepo.RetrieveAllAllocations();
            var tutors = from allocation in allocations
                         where idArray.Contains(allocation.Tutor.ID)
                         select allocation;
            return Ok(tutors);
        }

        [HttpGet]
        [Route("students")]
        public async Task<IHttpActionResult> ListAllStudents()
        {
            var students = await _authRepo.AllStudents();
            var response = students.Select(student => student.ToStudentResponseModel()).ToList();
            foreach (var student in response)
            {
                student.Tutor = _allocationRepo.FindTutorOfStudent(student.ID);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("student")]
        public async Task<IHttpActionResult> ListAllStudentsWithIds(string ids)
        {
            if (ids == null) return BadRequest();

            var idArray = ids.Split(',');
            var students = await _authRepo.FindStudentsByIds(idArray);
            var response = students.Select(student => student.ToStudentResponseModel()).ToList();
            foreach (var student in response)
            {
                student.Tutor = _allocationRepo.FindTutorOfStudent(student.ID);
            }
            return Ok(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _authRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
