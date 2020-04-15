using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO
{
    public class AllAllocationResponseModel
    {
        public IEnumerable<AllocationResponseModel> Allocations { get; set; }

        public IEnumerable<UserResponseModelDto> UnallocatedStudents { get; set; }
    }
}