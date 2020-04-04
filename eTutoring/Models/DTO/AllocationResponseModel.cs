using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO
{
    public class AllocationResponseModel
    {
        public UserResponseModelDto Tutor { get; set; }
        public IEnumerable<UserResponseModelDto> Students { get; set; }
    }
}