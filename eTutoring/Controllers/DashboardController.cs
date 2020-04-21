using eTutoring.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eTutoring.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        private readonly DashboardRepository _dashboardRepo = new DashboardRepository();

        [Authorize(Roles = "tutor")]
        [HttpGet]
        [Route("tutor-overview")]
        public IHttpActionResult GetTutorDashboardStats()
        {
            var tutorId = User.Identity.GetUserId();
            var report = _dashboardRepo.GetTutorStats(tutorId);
            return Ok(report);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dashboardRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
