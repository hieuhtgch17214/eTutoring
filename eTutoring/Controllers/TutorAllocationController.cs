﻿using eTutoring.Models.DTO;
using eTutoring.Models.DTO.FormModels;
using eTutoring.Repositories;
using System.Threading.Tasks;
using System.Web.Http;

namespace eTutoring.Controllers
{
    [RoutePrefix("api/allocation")]
    [Authorize(Roles = "staff")]
    public class TutorAllocationController : ApiController
    {
        private readonly TutorAllocationRepository _repo = new TutorAllocationRepository();

        [Route("add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddAllocation(TutorAllocationFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isSuccessful = await _repo.AllocateTutorToStudents(form.TutorId, form.StudentIds);
            if (isSuccessful)
            {
                var response = new
                {
                    message = "Allocation completed"
                };
                return Ok(response);
            }
            return BadRequest();
        }

        [Route("delete")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveAllocation(StudentDeallocationFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repo.DeallocateStudents(form.StudentIds);
            var response = new
            {
                message = "Deallocation completed"
            };
            return Ok(response);
        }

        [Route("view-allocations")]
        [HttpGet]
        public IHttpActionResult Allocations()
        {
            var result = _repo.RetrieveAllAllocations();
            return Ok(result);
        }

        [Route("unallocated-students")]
        [HttpGet]
        public async Task<IHttpActionResult> ViewUnallocatedStudents()
        {
            var result = await _repo.GetUnallocatedStudents();
            return Ok(result);
        }

        [Route("view-all-allocations")]
        [HttpGet]
        public async Task<IHttpActionResult> AllAllocations()
        {
            var allocations = _repo.RetrieveAllAllocations();
            var unallocatedStudents = await _repo.GetUnallocatedStudents();
            var result = new AllAllocationResponseModel
            {
                Allocations = allocations,
                UnallocatedStudents = unallocatedStudents
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
    }
}
