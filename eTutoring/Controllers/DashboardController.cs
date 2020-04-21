using eTutoring.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace eTutoring.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        private readonly DashboardRepository _dashboardRepo = new DashboardRepository();
        private readonly TutorAllocationRepository _allocationRepo = new TutorAllocationRepository();

        [Authorize(Roles = "tutor")]
        [HttpGet]
        [Route("tutor-overview")]
        public IHttpActionResult GetTutorDashboardStats()
        {
            var tutorId = User.Identity.GetUserId();
            var report = _dashboardRepo.GetTutorStats(tutorId);
            return Ok(report);
        }

        [Authorize(Roles = "tutor")]
        [HttpGet]
        [Route("my-students")]
        public async Task<IHttpActionResult> GetPersonalStudents()
        {
            var tutorId = User.Identity.GetUserId();
            var report = await _allocationRepo.RetrieveStudentsOfTutor(tutorId);
            return Ok(report);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dashboardRepo.Dispose();
                _allocationRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
