using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO
{
    public class StudentResponseModel : UserResponseModelDto
    {
        public UserResponseModelDto Tutor { get; set; }
    }
}