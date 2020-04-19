using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO
{
    public class FileCommentResponseModel
    {
        public int ID { get; set; }
        public UserResponseModelDto Author { get; set; }
        public string Comment { get; set; }
    }
}