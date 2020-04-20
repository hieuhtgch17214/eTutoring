using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO
{
    public class UserResponseModelDto
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}