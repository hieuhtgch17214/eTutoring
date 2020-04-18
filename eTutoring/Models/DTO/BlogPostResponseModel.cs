using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO
{
    public class BlogPostResponseModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}